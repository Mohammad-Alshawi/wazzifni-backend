using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wazzifni.Courses.Dto;
using Wazzifni.Courses;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Courses;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Wazzifni.Enums.Enum;
using Wazzifni.Authorization;
using Wazzifni.Companies.Dto;
using Wazzifni.Domain.Companies.Dto;
using Wazzifni.Domain.Companies;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Authorization.Users;
using AutoMapper;
using Wazzifni.Domain.Attachments;
using Abp.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Wazzifni.Configuration;
using Abp.Collections.Extensions;
using Wazzifni.Domain.CourseTags;
using Castle.MicroKernel;

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
        private readonly IAttachmentManager _attachmentManager;

        public CourseAppService(IRepository<Course, int> repository,UserManager userManager,
            IMapper mapper,
            ICourseManager courseManager,
            ICourseTagManager courseTagManager,
            IRepository<CourseTag> courseTagRepository,
            IAttachmentManager attachmentManager) : base(repository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _courseManager = courseManager;
            _courseTagManager = courseTagManager;
            _courseTagRepository = courseTagRepository;
            _attachmentManager = attachmentManager;
        }



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
    
            UnitOfWorkManager.Current.SaveChanges();

            foreach (var attachmentId in input.Attachments)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync( 
                    attachmentId, AttachmentRefType.Course, CourseId);
            }

            return _mapper.Map<CourseDetailsDto>(course);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        [HttpPut]

        public override async Task<CourseDetailsDto> UpdateAsync(UpdateCourseDto input)
        {
            var Course = await _courseManager.GetEntityByAsTrackingIdAsync(input.Id);
            
            Course.Translations.Clear();

            Course = _mapper.Map(input, Course);

            if (!input.TagsIds.IsNullOrEmpty())
            {
                var oldTags = Course.Tags.ToList();
                var newTags = new List<CourseTag>();
                foreach (var i in input.TagsIds)
                {
                    newTags.Add(await _courseTagManager.GetLiteEntityByIdAsync(i));
                }
                var TagsIdsToDelete = oldTags.Except(newTags).ToList();
                foreach (var CourseTag in TagsIdsToDelete)
                {
                    Course.Tags.Remove(CourseTag);
                    await _courseTagManager.UpdateCourseTag(CourseTag);
                }
                foreach (var CourseTag in newTags)
                {
                    if (!Course.Tags.Contains(CourseTag))
                    {
                        Course.Tags.Add(CourseTag);
                        await _courseTagManager.UpdateCourseTag(CourseTag);

                    }

                }
            }
            await Repository.UpdateAsync(Course);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var oldimagesAttachments = await _attachmentManager.GetByRefAsync(Course.Id, AttachmentRefType.Course);
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
                    attachmentId, AttachmentRefType.Course, Course.Id);
            }      

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return _mapper.Map<CourseDetailsDto>(Course);
        }

        public override async Task<CourseDetailsDto> GetAsync(EntityDto<int> input)
        {
            var Course = await _courseManager.GetFullEntityByIdAsync(input.Id);

            var result = _mapper.Map<CourseDetailsDto>(Course);

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
           
            return result;
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
            data = data.Include(x => x.CourseCategory).ThenInclude(x=>x.Translations);

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

            return data;
        }
        protected override IQueryable<Course> ApplySorting(IQueryable<Course> query, PagedCourseResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
