using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkPostFaveorites;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.WorkPosts.Dto;
using Wazzifni.WorkPosts.Notifications;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts
{
    public class WorkPostAppService :
         WazzifniAsyncCrudAppService<WorkPost, WorkPostDetailsDto, long, WorkPostLiteDto, PagedWorkPostResultRequestDto, CreateWorkPostDto, UpdateWorkPostDto>,
         IWorkPostAppService
    {
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        private readonly IWorkPostManager _workPostManager;
        private readonly IFavoriteWorkPostManager _favoriteWorkPostManager;
        private readonly IWorkApplicationManager _workApplicationManager;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IWorkPostNotificationService _workPostNotificationService;
        private readonly ICompanyManager _companyManager;

        public WorkPostAppService(IRepository<WorkPost, long> repository,
            IMapper mapper, UserManager userManager,
            IWorkPostManager workPostManager,
            IFavoriteWorkPostManager favoriteWorkPostManager,
            IWorkApplicationManager workApplicationManager,
            IAttachmentManager attachmentManager,
            IWorkPostNotificationService workPostNotificationService,
            ICompanyManager companyManager) : base(repository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _workPostManager = workPostManager;
            _favoriteWorkPostManager = favoriteWorkPostManager;
            _workApplicationManager = workApplicationManager;
            _attachmentManager = attachmentManager;
            _workPostNotificationService = workPostNotificationService;
            _companyManager = companyManager;
        }

        [HttpPost, AbpAuthorize(PermissionNames.WorkPosts_Create)]
        public override async Task<WorkPostDetailsDto> CreateAsync(CreateWorkPostDto input)
        {
            var post = _mapper.Map<WorkPost>(input);

            if (await _userManager.IsCompany())
            {
                var companyId = await _companyManager.GetCompanyIdByUserId(AbpSession.UserId.Value);
                post.CompanyId = companyId;
            }
            else post.CompanyId = input.CompanyId.Value;

            var company = await _companyManager.GetLiteCompanyByIdAsync(post.CompanyId);

            post.Company = company;
            post.Status = WorkPostStatus.Approved;
            post.WorkAvailbility = WorkAvailbility.Available;



            var lastPost = await Repository.GetAll().OrderByDescending(wp => wp.Slug).FirstOrDefaultAsync();

            int newSlugNumber = lastPost != null ? int.Parse(lastPost.Slug) + 1 : 1;
            post.Slug = newSlugNumber.ToString("D6"); // Format as 6 digits (e.g., 000001)


            await Repository.InsertAndGetIdAsync(post);
            UnitOfWorkManager.Current.SaveChanges();

            await _workPostNotificationService.SendNotificationForSendWorkPostToCompany(post);
            await _workPostNotificationService.SendNotificationForCreateWorkPostToAdmin(post);

            return _mapper.Map<WorkPostDetailsDto>(post);
        }


        [HttpGet]
        public override async Task<WorkPostDetailsDto> GetAsync(EntityDto<long> input)
        {
            var post = await _workPostManager.GetEntityByIdAsync(input.Id);

            var result = _mapper.Map<WorkPostDetailsDto>(post);

            if (AbpSession.UserId.HasValue)
            {
                result.IsFavorite = await _favoriteWorkPostManager.CheckIfWorkPostInFavoritesAsync(result.Id, AbpSession.UserId.Value);
                result.IsIApply = await _workApplicationManager.CheckIfWorkPostInApplicationUserAsync(result.Id, AbpSession.UserId.Value);

            }

            var profile = await _attachmentManager.GetElementByRefAsync(result.CompanyId, AttachmentRefType.CompanyLogo);

            if (profile is not null)
            {

                result.Company.Profile = new LiteAttachmentDto
                {
                    Id = profile.Id,
                    Url = _attachmentManager.GetUrl(profile),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(profile),
                };


            }

            return result;

        }



        [HttpPut, AbpAuthorize(PermissionNames.WorkPosts_Update)]


        public override async Task<WorkPostDetailsDto> UpdateAsync(UpdateWorkPostDto input)
        {
            var post = await _workPostManager.GetEntityWithoutUserByIdAsync(input.Id);

            var oldRequiredEmployeesCount = post.RequiredEmployeesCount;

            if (oldRequiredEmployeesCount > input.RequiredEmployeesCount)
            {
                throw new UserFriendlyException(Exceptions.DecreasingRequiredEmployeesNotAllowedException);
            }

            var oldCompanyId = post.CompanyId;

            _mapper.Map(input, post);

            if (await _userManager.IsCompany())
            {
                var companyId = await _companyManager.GetCompanyIdByUserId(AbpSession.UserId.Value);
                if (oldCompanyId != companyId)
                {
                    throw new UserFriendlyException("Denied");
                }
                post.CompanyId = companyId;
            }
            else
            {
                post.CompanyId = input.CompanyId.Value;
            }

            if (oldRequiredEmployeesCount < input.RequiredEmployeesCount)
            {
                post.IsClosed = false;
                post.WorkAvailbility = WorkAvailbility.Available;
            }

            await Repository.UpdateAsync(post);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return _mapper.Map<WorkPostDetailsDto>(post);
        }





        [AbpAuthorize]
        public async Task AddOrRemoveFromMyFavourites(EntityDto<long> input)
        {
            var WorkPost = await _workPostManager.GetEntityByIdAsync(input.Id);
            if (WorkPost is null)
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.WorkPost"));
            var isWorkPostInFavourite = await _favoriteWorkPostManager.CheckIfWorkPostInFavoritesAsync(input.Id, AbpSession.UserId.Value);
            if (!isWorkPostInFavourite)
            {
                FavoriteWorkPost favoriteWorkPost = new FavoriteWorkPost
                {
                    CreatorUserId = AbpSession.UserId.Value,
                    WorkPostId = input.Id
                };
                await _favoriteWorkPostManager.AddWorkPostToFavouriteAsync(favoriteWorkPost);
            }
            await _favoriteWorkPostManager.DeleteWorkPostFromFavouriteAsync(input.Id, AbpSession.UserId.Value);
        }




        [AbpAllowAnonymous]
        public override async Task<PagedResultDto<WorkPostLiteDto>> GetAllAsync(PagedWorkPostResultRequestDto input)
        {


            var result = await base.GetAllAsync(input);

            var ItemsIds = result.Items.Select(x => x.Id).ToList();
            var favoritePostIds = new HashSet<long>();
            var appliedPostIds = new HashSet<long>();

            if (AbpSession.UserId.HasValue)
            {
                favoritePostIds = await _favoriteWorkPostManager.GetUserFavoriteWorkPostIdsAsync(AbpSession.UserId.Value, ItemsIds);
                appliedPostIds = await _workApplicationManager.GetUserAppliedWorkPostIdsAsync(AbpSession.UserId.Value, ItemsIds);
            }
            foreach (var item in result.Items)
            {
                if (AbpSession.UserId.HasValue && favoritePostIds.Count > 0)
                    item.IsFavorite = favoritePostIds.Contains(item.Id);

                if (AbpSession.UserId.HasValue && appliedPostIds.Count > 0)
                    item.IsIApply = appliedPostIds.Contains(item.Id);
            }

            return result;

        }

        protected override IQueryable<WorkPost> CreateFilteredQuery(PagedWorkPostResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);



            if (!string.IsNullOrEmpty(input.Keyword))
            {
                var keyword = input.Keyword.ToLower();
                var matchingEducationLevels = Enum.GetValues<EducationLevel>()
                                                  .Where(e => e.ToString().ToLower().Contains(keyword))
                                                  .ToList();
                var matchingWorkEngagements = Enum.GetValues<WorkEngagement>()
                                                  .Where(e => e.ToString().ToLower().Contains(keyword))
                                                  .ToList();
                var matchingWorkLevels = Enum.GetValues<WorkLevel>()
                                                  .Where(e => e.ToString().ToLower().Contains(keyword))
                                                  .ToList();

                data = data.Where(wp =>
                    wp.Title.Contains(input.Keyword) ||
                    wp.Description.Contains(input.Keyword) ||
                    matchingEducationLevels.Contains(wp.EducationLevel) ||
                    matchingWorkEngagements.Contains(wp.WorkEngagement) ||
                    matchingWorkLevels.Contains(wp.WorkLevel) ||
                    wp.Company.Translations.Any(t => EF.Functions.Like(t.Name, $"%{input.Keyword}%")) ||
                    wp.Company.City.Translations.Any(t => EF.Functions.Like(t.Name, $"%{input.Keyword}%"))
                );
            }

            if (AbpSession.UserId.HasValue && input.IsFavorite.HasValue && input.IsFavorite.Value)
            {
                data = _favoriteWorkPostManager.GetFavoriteWorkPostsQueryByUserIdAsync(AbpSession.UserId.Value);
            }

            if (AbpSession.UserId.HasValue && input.IsIApply.HasValue && input.IsIApply.Value)
            {
                data = _workApplicationManager.GetApplyWorkPostsQueryByUserIdAsync(AbpSession.UserId.Value);
            }

            if (input.CompanyId.HasValue)
                data = data.Where(wp => wp.CompanyId == input.CompanyId.Value);



            if (input.Status.HasValue)
                data = data.Where(wp => wp.Status == input.Status.Value);

            if (input.WorkPlace.HasValue)
                data = data.Where(wp => wp.WorkPlace == input.WorkPlace.Value);

            if (input.WorkEngagement.HasValue)
                data = data.Where(wp => wp.WorkEngagement == input.WorkEngagement.Value);

            if (input.WorkLevel.HasValue)
                data = data.Where(wp => wp.WorkLevel == input.WorkLevel.Value);

            if (input.EducationLevel.HasValue)
                data = data.Where(wp => wp.EducationLevel == input.EducationLevel.Value);

            if (input.MinSalary.HasValue)
                data = data.Where(wp => wp.MinSalary >= input.MinSalary.Value);

            if (input.MaxSalary.HasValue)
                data = data.Where(wp => wp.MaxSalary <= input.MaxSalary.Value);

            if (input.WorkAvailbility.HasValue)
                data = data.Where(wp => wp.WorkAvailbility == input.WorkAvailbility.Value);

            if (input.ProfileId.HasValue)
                data = data.Where(wp => wp.Applications.Any(x => x.ProfileId == input.ProfileId.Value));

            if (input.MinExperience.HasValue)
            {
                data = data.Where(j => j.ExperienceYearsCount >= input.MinExperience);
            }

            if (input.MaxExperience.HasValue)
            {
                data = data.Where(j => j.ExperienceYearsCount <= input.MaxExperience);
            }


            if (input.TimeFilter.HasValue)
            {
                data = FilterOnTime(input, data);
            }


            data = data.Include(x => x.Company).ThenInclude(x => x.User);
            data = data.Include(x => x.Company).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Company).ThenInclude(x => x.City).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Applications);

            if (input.CityId.HasValue)
                data = data.Where(wp => wp.Company.CityId == input.CityId.Value);

            return data;
        }

        private static IQueryable<WorkPost> FilterOnTime(PagedWorkPostResultRequestDto input, IQueryable<WorkPost> data)
        {
            var now = DateTime.Now;
            switch (input.TimeFilter)
            {
                case Enums.Enum.FilterOnTime.ThisToday:
                    data = data.Where(j => j.CreationTime.Date == now.Date);
                    break;

                case Enums.Enum.FilterOnTime.ThisWeek:
                    DateTime sevenDaysAgo = now.AddDays(-7);
                    data = data.Where(j => j.CreationTime >= sevenDaysAgo);
                    break;

                case Enums.Enum.FilterOnTime.ThisMonth:
                    data = data.Where(j => j.CreationTime.Year == now.Year && j.CreationTime.Month == now.Month);
                    break;
            }

            return data;
        }

        protected override IQueryable<WorkPost> ApplySorting(IQueryable<WorkPost> query, PagedWorkPostResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
