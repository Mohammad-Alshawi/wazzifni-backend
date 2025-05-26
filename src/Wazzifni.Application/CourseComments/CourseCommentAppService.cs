using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CourseComments.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.CourseComments;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.Trainees;
using Wazzifni.Localization.SourceFiles;

namespace Wazzifni.CourseComments
{

    public class CourseCommentAppService :
        WazzifniAsyncCrudAppService<CourseComment, CourseCommentDetailsDto, long, CourseCommentLiteDto, PagedCourseCommentResultRequestDto, CreateCourseCommentDto, UpdateCourseCommentDto>,
        ICourseCommentAppService
    {


        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<CourseComment, long> _CourseCommentRepository;
        private readonly ICourseManager _courseManager;
        private readonly ITraineeManager _traineeManager;
        private readonly IAttachmentManager _attachmentManager;


        public CourseCommentAppService(
            UserManager userManager,
            ICacheManager cacheManager,
            CountryManager countryManager,
            IRepository<CourseComment, long> courseCommentRepository,
            ICourseManager courseManager,
            ITraineeManager traineeManager,
            IAttachmentManager attachmentManager
        ) : base(courseCommentRepository)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _CourseCommentRepository = courseCommentRepository;
            _courseManager = courseManager;
            _traineeManager = traineeManager;
            _attachmentManager = attachmentManager;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<CourseCommentDetailsDto> GetAsync(EntityDto<long> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<CourseCommentLiteDto>> GetAllAsync(PagedCourseCommentResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        [AbpAuthorize(PermissionNames.CourseComment_Create)]
        public override async Task<CourseCommentDetailsDto> CreateAsync(CreateCourseCommentDto input)
        {



            var CourseComment = ObjectMapper.Map<CourseComment>(input);

            CourseComment.CreationTime = DateTime.UtcNow;
            CourseComment.UserId = AbpSession.UserId.Value;

            await Repository.InsertAsync(CourseComment);
            await CurrentUnitOfWork.SaveChangesAsync();

            /* if (input.AttachmentId != 0)
             {
                 await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.CourseComment, CourseComment.Id);
             }*/


            return MapToEntityDto(CourseComment);
        }


        [AbpAuthorize(PermissionNames.CourseComment_Update)]
        public override async Task<CourseCommentDetailsDto> UpdateAsync(UpdateCourseCommentDto input)
        {
            var CourseComment = await _CourseCommentRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();

            if (CourseComment is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.CourseComment"));
            }
            if (AbpSession.UserId.Value != CourseComment?.UserId)
            {
                throw new UserFriendlyException(string.Format(Exceptions.YouCannotDoThisAction));
            }

            MapToEntity(input, CourseComment);

            CourseComment.UserId = AbpSession.UserId.Value;

            await _CourseCommentRepository.UpdateAsync(CourseComment);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return MapToEntityDto(CourseComment);

        }

        [AbpAuthorize(PermissionNames.CourseComment_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var CourseComment = await _CourseCommentRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();

            if (CourseComment is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.CourseComment"));
            }
            if (!await _userManager.IsAdminSession() && AbpSession.UserId.Value != CourseComment?.UserId)
            {
                throw new UserFriendlyException(string.Format(Exceptions.YouCannotDoThisAction));
            }


            await _CourseCommentRepository.DeleteAsync(input.Id);
        }






        private async Task<CourseCommentDetailsDto> GetFromDatabase(EntityDto<long> input)
        {
            var CourseComment = await _CourseCommentRepository.GetAll().Include(x => x.Course).ThenInclude(x => x.Translations)
                .Include(x => x.User).ThenInclude(x => x.Profile)
                .Include(x => x.User).ThenInclude(x => x.Company)
                .Where(x => x.Id == input.Id).FirstOrDefaultAsync();

            var CourseCommentDetailsDto = MapToEntityDto(CourseComment);

            return CourseCommentDetailsDto;
        }

        private async Task<PagedResultDto<CourseCommentLiteDto>> GetAllFromDatabase(PagedCourseCommentResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var ItemsIds = result.Items.Select(x => x.Id).ToList();

            //long traineeId = 0;

            //if (AbpSession.UserId.HasValue) traineeId = await _traineeManager.GetTraineeIdByUserId(AbpSession.UserId.Value);

            foreach (var item in result.Items)
            {
                if (AbpSession.UserId.HasValue && !await _userManager.IsAdminSession())
                {
                    item.IsForMe = item.UserId == AbpSession.UserId.Value;
                }
            }


            return result;
        }




        protected override IQueryable<CourseComment> CreateFilteredQuery(PagedCourseCommentResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.Course).ThenInclude(x => x.Translations).Include(x => x.User).ThenInclude(x => x.Profile).Include(x => x.User).ThenInclude(x => x.Company);

            if (input.CourseId.HasValue)
                data = data.Where(x => x.CourseId == input.CourseId.Value);
            /*
                        if (input.TraineeId.HasValue)
                            data = data.Where(x => x.TraineeId == input.TraineeId.Value);*/

            if (input.UserId.HasValue)
                data = data.Where(x => x.UserId == input.UserId.Value);

            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Content.Contains(input.Keyword));


            return data;
        }

        protected override IQueryable<CourseComment> ApplySorting(IQueryable<CourseComment> query, PagedCourseCommentResultRequestDto input)
        {

            return query.OrderBy(r => r.Id);
        }
    }
}
