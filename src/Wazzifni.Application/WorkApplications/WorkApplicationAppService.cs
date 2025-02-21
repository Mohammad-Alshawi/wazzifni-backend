using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.WorkApplications.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkApplications
{
    public class WorkApplicationAppService :
         WazzifniAsyncCrudAppService<WorkApplication, WorkApplicationDetailsDto, long, WorkApplicationLiteDto, PagedWorkApplicationResultRequestDto, CreateWorkApplicationDto, UpdateWorkApplicationDto>,
         IWorkApplicationAppService
    {
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        private readonly IProfileManager _profileManager;
        private readonly IWorkPostManager _workPostManager;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IWorkApplicationManager _workApplicationManager;
        private readonly ICompanyManager _companyManager;

        public WorkApplicationAppService(IRepository<WorkApplication, long> repository,
            IMapper mapper, UserManager userManager,
            IProfileManager profileManager,
            IWorkPostManager workPostManager,
            IAttachmentManager attachmentManager,
            IWorkApplicationManager workApplicationManager,
            ICompanyManager companyManager) : base(repository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _profileManager = profileManager;
            _workPostManager = workPostManager;
            _attachmentManager = attachmentManager;
            _workApplicationManager = workApplicationManager;
            _companyManager = companyManager;
        }

        [HttpPost, AbpAuthorize(PermissionNames.WorkApplications_Create)]
        public override async Task<WorkApplicationDetailsDto> CreateAsync(CreateWorkApplicationDto input)
        {
            var application = _mapper.Map<WorkApplication>(input);
            var currentProfileId = await _profileManager.GetProfileIdByUserId(AbpSession.UserId.Value);
            var workPost = await _workPostManager.GetEntityByIdAsTrackingAsync(input.WorkPostId);

            application.Status = WorkApplicationStatus.Pending;
            application.Description = input.Description;
            application.ProfileId = currentProfileId;

            workPost.ApplicantsCount++;

            if (workPost.ApplicantsCount >= workPost.RequiredEmployeesCount)
                workPost.WorkAvailbility = WorkAvailbility.Unavilable;

            await Repository.InsertAndGetIdAsync(application);
            UnitOfWorkManager.Current.SaveChanges();

            return _mapper.Map<WorkApplicationDetailsDto>(application);
        }


        [HttpGet]
        public override async Task<WorkApplicationDetailsDto> GetAsync(EntityDto<long> input)
        {
            var application = await _workApplicationManager.GetEntityByIdAsync(input.Id);

            return _mapper.Map<WorkApplicationDetailsDto>(application);
        }



        [HttpPut, ApiExplorerSettings(IgnoreApi = true), RemoteService(IsEnabled = false)]


        public override async Task<WorkApplicationDetailsDto> UpdateAsync(UpdateWorkApplicationDto input)
        {
            var application = await _workApplicationManager.GetEntityByIdAsync(input.Id);

            var oldWorkPostId = application.WorkPostId;

            _mapper.Map(input, application);

            await Repository.UpdateAsync(application);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return _mapper.Map<WorkApplicationDetailsDto>(application);
        }

        [HttpDelete, AbpAuthorize(PermissionNames.WorkApplications_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var application = await _workApplicationManager.GetEntityByIdAsTrackingAsync(input.Id);
            if (application == null)
                throw new UserFriendlyException("Application not found.");

            var workPost = await _workPostManager.GetEntityByIdAsTrackingAsync(application.WorkPostId);
            var currentUserId = AbpSession.UserId.Value;
            var currentProfileId = await _profileManager.GetProfileIdByUserId(currentUserId);
            var isCompanyOwner = await _companyManager.IsUserCompanyOwner(currentUserId, workPost.CompanyId);
            var isApplicant = application.ProfileId == currentProfileId;

            if (!(isApplicant || isCompanyOwner))
                throw new UserFriendlyException("You are not authorized to delete this application.");

            if (application.Status == WorkApplicationStatus.Approved)
                throw new UserFriendlyException(Exceptions.DeleteApprovedApplication);

            if (application.Status == WorkApplicationStatus.Pending)
            {
                if (isApplicant)
                {
                    workPost.ApplicantsCount--;
                    if (workPost.ApplicantsCount < workPost.RequiredEmployeesCount)
                        workPost.WorkAvailbility = WorkAvailbility.Available;
                    await Repository.DeleteAsync(application);
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                    return;
                }
                if (isCompanyOwner)
                {
                    throw new UserFriendlyException(Exceptions.PendingApplicationCanDeleteByCompany);

                }
            }
            if (application.Status == WorkApplicationStatus.Rejected)
            {
                if (isCompanyOwner)
                {
                    application.DeletedByCompany = true;
                    await Repository.UpdateAsync(application);
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                    return;
                }
                if (isApplicant)
                {
                    await Repository.DeleteAsync(application);
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                    return;
                }
            }
        }

        [HttpPost, AbpAuthorize(PermissionNames.WorkApplications_Approve)]

        public async Task<WorkApplicationDetailsDto> Approve(ApproveWorkApplicationDto input)
        {
            var application = await _workApplicationManager.GetEntityByIdAsTrackingAsync(input.Id);
            var workPost = await _workPostManager.GetEntityByIdAsTrackingAsync(application.WorkPostId);

            application.Status = WorkApplicationStatus.Approved;
            workPost.Applications.Where(x => x.Id == input.Id).FirstOrDefault().Status = WorkApplicationStatus.Approved;

            if (workPost.ApplicantsCount >= workPost.RequiredEmployeesCount && workPost.Applications.Where(x => x.Status == WorkApplicationStatus.Approved).Count() == workPost.RequiredEmployeesCount)
                workPost.IsClosed = true;

            await Repository.UpdateAsync(application);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return _mapper.Map<WorkApplicationDetailsDto>(application);
        }


        [HttpPost, AbpAuthorize(PermissionNames.WorkApplications_Reject)]

        public async Task<WorkApplicationDetailsDto> Reject(RejectWorkApplicationDto input)
        {
            var application = await _workApplicationManager.GetEntityByIdAsTrackingAsync(input.Id);

            var workPost = await _workPostManager.GetEntityByIdAsTrackingAsync(application.WorkPostId);


            application.Status = WorkApplicationStatus.Rejected;
            application.RejectReason = input.RejectReason;

            workPost.Applications.Where(x => x.Id == input.Id).FirstOrDefault().Status = WorkApplicationStatus.Rejected;

            workPost.ApplicantsCount--;

            if (workPost.ApplicantsCount < workPost.RequiredEmployeesCount)
                workPost.WorkAvailbility = WorkAvailbility.Available;


            await Repository.UpdateAsync(application);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return _mapper.Map<WorkApplicationDetailsDto>(application);
        }



        [AbpAllowAnonymous]
        public override async Task<PagedResultDto<WorkApplicationLiteDto>> GetAllAsync(PagedWorkApplicationResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Profile.Id).ToList(), AttachmentRefType.Profile);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Profile.Id, out var itemAttachments))
                {
                    item.Profile.Image = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }


            }
            return result;
        }

        protected override IQueryable<WorkApplication> CreateFilteredQuery(PagedWorkApplicationResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.Profile).ThenInclude(x => x.User);
            data = data.Include(x => x.WorkPost).ThenInclude(x => x.Company).ThenInclude(x => x.Translations);

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                data = data.Where(wp =>

                    wp.Description.Contains(input.Keyword) ||
                    wp.RejectReason.Contains(input.Keyword) ||
                    wp.Status.ToString().Contains(input.Keyword) ||
                    wp.WorkPost.Company.Translations.Any(t => t.Name.Contains(input.Keyword)) ||
                    wp.WorkPost.Title.Contains(input.Keyword) ||
                    wp.WorkPost.Description.Contains(input.Keyword) ||
                    wp.Profile.User.Name.Contains(input.Keyword) ||
                    wp.Profile.User.Surname.Contains(input.Keyword)

                );
            }

            if (input.DeletedByCompany.HasValue)
                data = data.Where(wp => wp.DeletedByCompany == input.DeletedByCompany.Value);

            if (input.WorkPostId.HasValue)
                data = data.Where(wp => wp.WorkPostId == input.WorkPostId.Value);

            if (input.Status.HasValue)
                data = data.Where(wp => wp.Status == input.Status.Value);

            if (input.CompanyId.HasValue)
                data = data.Where(wp => wp.WorkPost.CompanyId == input.CompanyId.Value);

            if (input.ProfileId.HasValue)
                data = data.Where(wp => wp.ProfileId == input.ProfileId.Value);

            return data;
        }
        protected override IQueryable<WorkApplication> ApplySorting(IQueryable<WorkApplication> query, PagedWorkApplicationResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
