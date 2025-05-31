using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.Courses.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.CourseTags;
using Wazzifni.Domain.Teachers;
using Wazzifni.Domain.Trainees;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Courses
{
    public class CourseAppService :
        WazzifniAsyncCrudAppService<Course, CourseDetailsDto, int, CourseLiteDto, PagedCourseResultRequestDto, CreateCourseDto, UpdateCourseDto>,
        ICourseAppService
    {
        private readonly UserManager _userManager;
        private readonly IMapper _mapper;
        private readonly ICourseManager _courseManager;
        private readonly ICourseTagManager _courseTagManager;
        private readonly IRepository<CourseTag> _courseTagRepository;
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly ITraineeManager _traineeManager;
        private readonly IAttachmentManager _attachmentManager;

        public CourseAppService(IRepository<Course, int> repository, UserManager userManager,
            IMapper mapper,
            ICourseManager courseManager,
            ICourseTagManager courseTagManager,
            IRepository<CourseTag> courseTagRepository,
            IRepository<Teacher> teacherRepository,
            ITraineeManager traineeManager,
            IAttachmentManager attachmentManager) : base(repository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _courseManager = courseManager;
            _courseTagManager = courseTagManager;
            _courseTagRepository = courseTagRepository;
            _teacherRepository = teacherRepository;
            _traineeManager = traineeManager;
            _attachmentManager = attachmentManager;
        }


        [AbpAuthorize]
        [HttpPost]
        public override async Task<CourseDetailsDto> CreateAsync(CreateCourseDto input)
        {

            var userLogin = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

            var course = _mapper.Map<Course>(input);
            ;
            if (!input.TagsIds.IsNullOrEmpty())
            {

                var tags = await _courseTagRepository.GetAll()
                    .Where(t => input.TagsIds.Contains(t.Id))
                    .ToListAsync();

                course.Tags = tags;
            }

            var CourseId = await Repository.InsertAndGetIdAsync(course);

            var teacher = await _teacherRepository.GetAll()
               .Where(t => t.Id == input.TeacherId)
               .FirstOrDefaultAsync();

            teacher.AssignedCourseCount++;

            UnitOfWorkManager.Current.SaveChanges();

            foreach (var attachmentId in input.Attachments)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                    attachmentId, AttachmentRefType.Course, CourseId);
            }

            return _mapper.Map<CourseDetailsDto>(course);
        }



        public async override Task DeleteAsync(EntityDto<int> input)
        {
            var course = await Repository.GetAllIncluding(
                c => c.Comments,
                c => c.CourseRegistrationRequests,
                c => c.Translations,
                c => c.Tags,
                c => c.CourseCategory,
                c => c.Teacher,
                c => c.City
            ).FirstOrDefaultAsync(c => c.Id == input.Id);

            if (course == null)
            {
                throw new UserFriendlyException("Not Found");
            }

            course.Comments.Clear();
            course.CourseRegistrationRequests.Clear();
            course.Translations.Clear();
            course.Tags.Clear();
            await Repository.DeleteAsync(course);

            var teacher = await _teacherRepository.GetAll()
              .Where(t => t.Id == course.TeacherId)
              .FirstOrDefaultAsync();

            teacher.AssignedCourseCount--;

            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        [HttpPut]

        public override async Task<CourseDetailsDto> UpdateAsync(UpdateCourseDto input)
        {
            var course = await _courseManager.GetEntityByAsTrackingIdAsync(input.Id);

            var oldSeatsNumber = course.NumberOfSeats;
            var oldTeacherId = course.TeacherId;

            course.Translations.Clear();

            course = _mapper.Map(input, course);

            if (course.NumberOfSeats > oldSeatsNumber && course.IsClosed)
            {
                course.IsClosed = false;
                course.ClosedDate = null;
            }
            if (!input.TagsIds.IsNullOrEmpty())
            {
                var oldTags = course.Tags.ToList();
                var newTags = new List<CourseTag>();
                foreach (var i in input.TagsIds)
                {
                    newTags.Add(await _courseTagManager.GetLiteEntityByIdAsync(i));
                }
                var TagsIdsToDelete = oldTags.Except(newTags).ToList();
                foreach (var CourseTag in TagsIdsToDelete)
                {
                    course.Tags.Remove(CourseTag);
                    await _courseTagManager.UpdateCourseTag(CourseTag);
                }
                foreach (var CourseTag in newTags)
                {
                    if (!course.Tags.Contains(CourseTag))
                    {
                        course.Tags.Add(CourseTag);
                        await _courseTagManager.UpdateCourseTag(CourseTag);

                    }

                }
            }


            if (input.TeacherId != oldTeacherId)
            {
                var oldTeacher = await _teacherRepository.GetAsync(oldTeacherId);
                if (oldTeacher.AssignedCourseCount > 0)
                    oldTeacher.AssignedCourseCount--;

                var newTeacher = await _teacherRepository.GetAsync(input.TeacherId);
                newTeacher.AssignedCourseCount++;
            }
            await Repository.UpdateAsync(course);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var oldimagesAttachments = await _attachmentManager.GetByRefAsync(course.Id, AttachmentRefType.Course);
            var oldimagesAttachmentsIds = oldimagesAttachments.Select(x => x.Id).ToList();
            var imagesattachmentsToDelete = oldimagesAttachments.Where(x => !input.Attachments.Contains((x.Id)));
            var imagesattachmentIdsToAdd = input.Attachments.Except(oldimagesAttachments.Select(x => x.Id).ToList());
            foreach (var attachment in imagesattachmentsToDelete)
            {
                await _attachmentManager.DeleteRefIdAsync(attachment);
            }
            foreach (var attachmentId in imagesattachmentIdsToAdd)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                    attachmentId, AttachmentRefType.Course, course.Id);
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return _mapper.Map<CourseDetailsDto>(course);
        }

        public override async Task<CourseDetailsDto> GetAsync(EntityDto<int> input)
        {
            var Course = await _courseManager.GetFullEntityByIdAsync(input.Id);

            var courseRigsteredIds = new HashSet<int>();

            var result = _mapper.Map<CourseDetailsDto>(Course);

            if (AbpSession.UserId.HasValue)
            {
                result.OldRate = await _courseManager.GetCourseRateForUser(AbpSession.UserId.Value, Course.Id);

                courseRigsteredIds = await _courseManager.GetCourseIdsUserIsRigesteredAsync(AbpSession.UserId.Value, new List<int> { result.Id });

                result.IRegistered = courseRigsteredIds.Contains(result.Id);

            }
            var attachments = await _attachmentManager.GetByRefAsync(input.Id, AttachmentRefType.Course);
            if (attachments is not null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        result.Images.Add(new LiteAttachmentDto
                        {
                            Id = attachment.Id,
                            Url = _attachmentManager.GetUrl(attachment),
                            LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                        });
                    }
                }
            }
            var attachmentsTeacher = await _attachmentManager.GetElementByRefAsync(result.Teacher.Id, AttachmentRefType.Teacher);
            if (attachmentsTeacher is not null)
            {

                result.Teacher.Image = new LiteAttachmentDto
                {
                    Id = attachmentsTeacher.Id,
                    Url = _attachmentManager.GetUrl(attachmentsTeacher),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachmentsTeacher),
                };

            }


            return result;
        }

        [HttpPost, AbpAuthorize(PermissionNames.Courses_Rate)]
        public async Task<dynamic> Rate(RateCourseDto input)
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var Course = await _courseManager.GetLiteCourseByIdAsync(input.CourseId);

            await _courseManager.RateForCourseByUserId(user.Id, Course.Id, input.Rate);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            Course.AverageRating = await _courseManager.GetAverageRatingForCourse(Course.Id);
            await Repository.UpdateAsync(Course);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return new { Result = true };
        }

        public override async Task<PagedResultDto<CourseLiteDto>> GetAllAsync(PagedCourseResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Id).ToList(), AttachmentRefType.Course);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Id, out var itemAttachments))
                {
                    item.Image = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }


            }
            return result;
        }

        protected override IQueryable<Course> CreateFilteredQuery(PagedCourseResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.Translations);
            data = data.Include(x => x.City).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Teacher);
            data = data.Include(x => x.Tags).ThenInclude(x => x.Translations);
            data = data.Include(x => x.CourseCategory).ThenInclude(x => x.Translations);
            data = data.Include(x => x.CourseRegistrationRequests);

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                var keyword = input.Keyword.ToLower();

                data = data.Where(c =>
                    c.Translations.Any(t =>
                        t.Title.ToLower().Contains(keyword) ||
                        t.Description.ToLower().Contains(keyword)
                    )
                );
            }

            if (input.CityId.HasValue)
                data = data.Where(x => x.CityId == input.CityId.Value);

            if (input.CourseCategoryId.HasValue)
                data = data.Where(x => x.CourseCategoryId == input.CourseCategoryId.Value);

            if (input.TeacherId.HasValue)
                data = data.Where(x => x.TeacherId == input.TeacherId.Value);

            if (input.TagsId != null && input.TagsId.Any())
                data = data.Where(x => x.Tags.Any(tag => input.TagsId.Contains(tag.Id)));

            if (input.Mode.HasValue)
                data = data.Where(x => x.Mode == input.Mode.Value);

            if (input.Difficulty.HasValue)
                data = data.Where(x => x.Difficulty == input.Difficulty.Value);

            if (input.IsFree.HasValue)
                data = data.Where(x => !x.Price.HasValue == input.IsFree.Value);

            if (input.IsFeatured.HasValue)
                data = data.Where(x => x.IsFeatured == input.IsFeatured.Value);


            if (input.UserId.HasValue)
                data = data.Where(x => x.CourseRegistrationRequests.Any(cr => input.UserId == cr.UserId));

            if (input.MinPrice.HasValue)
                data = data.Where(x => x.Price.HasValue && x.Price.Value >= input.MinPrice.Value);

            if (input.MaxPrice.HasValue)
                data = data.Where(x => x.Price.HasValue && x.Price.Value <= input.MaxPrice.Value);

            return data;
        }
        protected override IQueryable<Course> ApplySorting(IQueryable<Course> query, PagedCourseResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
