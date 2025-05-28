
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CourseRegistrationRequests.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.CourseRegistrationRequests;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.Trainees;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.CourseRegistrationRequests
{
    public class CourseRegistrationRequestAppService :
         WazzifniAsyncCrudAppService<CourseRegistrationRequest, CourseRegistrationRequestDetailsDto, long, CourseRegistrationRequestLiteDto, PagedCourseRegistrationRequestResultRequestDto, CreateCourseRegistrationRequestDto, UpdateCourseRegistrationRequestDto>,
         ICourseRegistrationRequestAppService
    {
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        private readonly ITraineeManager _TraineeManager;
        private readonly ICourseManager _CourseManager;
        private readonly IAttachmentManager _attachmentManager;
        private readonly ICourseRegistrationRequestManager _CourseRegistrationRequestManager;
        // private readonly ICourseRegistrationRequestNotificationsAppService _CourseRegistrationRequestNotificationsAppService;
        private readonly ICompanyManager _companyManager;

        public CourseRegistrationRequestAppService(IRepository<CourseRegistrationRequest, long> repository,
            IMapper mapper, UserManager userManager,
            ITraineeManager TraineeManager,
            ICourseManager CourseManager,
            IAttachmentManager attachmentManager,
            ICourseRegistrationRequestManager CourseRegistrationRequestManager,
            //ICourseRegistrationRequestNotificationsAppService CourseRegistrationRequestNotificationsAppService,
            ICompanyManager companyManager) : base(repository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _TraineeManager = TraineeManager;
            _CourseManager = CourseManager;
            _attachmentManager = attachmentManager;
            _CourseRegistrationRequestManager = CourseRegistrationRequestManager;
            // _CourseRegistrationRequestNotificationsAppService = CourseRegistrationRequestNotificationsAppService;
            _companyManager = companyManager;
        }

        [HttpPost, AbpAuthorize(PermissionNames.CourseRegistrationRequests_Create)]
        public override async Task<CourseRegistrationRequestDetailsDto> CreateAsync(CreateCourseRegistrationRequestDto input)
        {
            var registrationRequest = _mapper.Map<CourseRegistrationRequest>(input);
            var Course = await _CourseManager.GetLiteCourseByIdAsync(input.CourseId);

            if (!input.IsSpecial) registrationRequest.Status = CourseRegistrationRequestStatus.Checking;
            registrationRequest.IsSpecial = input.IsSpecial;

            registrationRequest.UserId = AbpSession.UserId.Value;


            if (await _userManager.IsCompany() && input.NumberOfRegisteredPeople is null)
                throw new UserFriendlyException("NumberOfRegisteredPeople is required");

            if (await _userManager.IsBasicUser())
                registrationRequest.NumberOfRegisteredPeople = 1;

            var id = await Repository.InsertAndGetIdAsync(registrationRequest);
            UnitOfWorkManager.Current.SaveChanges();

            var CourseRegistrationRequest = await _CourseRegistrationRequestManager.GetEntityByIdAsync(id);

            //await _CourseRegistrationRequestNotificationsAppService.SendNotificationForNewCourseRegistrationRequestToCompany(CourseRegistrationRequest);
            //await _CourseRegistrationRequestNotificationsAppService.SendNotificationForSendCourseRegistrationRequestToOwner(CourseRegistrationRequest);
            //await _CourseRegistrationRequestNotificationsAppService.SendNotificationForNewCourseRegistrationRequestToAdmin(CourseRegistrationRequest);


            return _mapper.Map<CourseRegistrationRequestDetailsDto>(registrationRequest);
        }


        [HttpGet]
        public override async Task<CourseRegistrationRequestDetailsDto> GetAsync(EntityDto<long> input)
        {
            var registrationRequest = await _CourseRegistrationRequestManager.GetEntityByIdAsync(input.Id);

            return _mapper.Map<CourseRegistrationRequestDetailsDto>(registrationRequest);
        }



        [HttpPut, ApiExplorerSettings(IgnoreApi = true), RemoteService(IsEnabled = false)]
        public override async Task<CourseRegistrationRequestDetailsDto> UpdateAsync(UpdateCourseRegistrationRequestDto input)
        {
            var registrationRequest = await _CourseRegistrationRequestManager.GetEntityByIdAsync(input.Id);

            var oldCourseId = registrationRequest.CourseId;

            _mapper.Map(input, registrationRequest);

            await Repository.UpdateAsync(registrationRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return _mapper.Map<CourseRegistrationRequestDetailsDto>(registrationRequest);
        }

        [HttpDelete, AbpAuthorize(PermissionNames.CourseRegistrationRequests_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var registrationRequest = await _CourseRegistrationRequestManager.GetEntityByIdAsTrackingAsync(input.Id);
            if (registrationRequest == null)
                throw new UserFriendlyException("registrationRequest not found.");

            var Course = await _CourseManager.GetLiteCourseByIdAsync(registrationRequest.CourseId);
            /*      var currentUserId = AbpSession.UserId.Value;
                  var currentTraineeId = await _TraineeManager.GetTraineeIdByUserId(currentUserId);
                  var isOwner = registrationRequest.TraineeId == currentTraineeId;*/

            if (registrationRequest.UserId != AbpSession.UserId.Value && !await _userManager.IsAdminSession())
                throw new UserFriendlyException("You are not authorized to delete this registrationRequest.");

            if (registrationRequest.Status == CourseRegistrationRequestStatus.Approved)
                throw new UserFriendlyException(Exceptions.DeleteApprovedregistrationRequest);

            if (registrationRequest.Status == CourseRegistrationRequestStatus.Rejected)
                throw new UserFriendlyException(Exceptions.ObjectCantBeDelete);

            if (registrationRequest.Status == CourseRegistrationRequestStatus.Checking)
            {
                await Repository.DeleteAsync(registrationRequest);
                await UnitOfWorkManager.Current.SaveChangesAsync();
                return;
            }

        }


        [HttpPost, AbpAuthorize(PermissionNames.CourseRegistrationRequests_Approve)]

        public async Task<CourseRegistrationRequestDetailsDto> Approve(ApproveCourseRegistrationRequestDto input)
        {
            var registrationRequest = await _CourseRegistrationRequestManager.GetEntityByIdAsTrackingAsync(input.Id);
            var Course = await _CourseManager.GetEntityByAsTrackingIdAsync(registrationRequest.CourseId);

            registrationRequest.Status = CourseRegistrationRequestStatus.Approved;
            Course.CourseRegistrationRequests.Where(x => x.Id == input.Id).FirstOrDefault().Status = CourseRegistrationRequestStatus.Approved;
            Course.RegisteredCount = Course.RegisteredCount + registrationRequest.NumberOfRegisteredPeople;

            if (Course.RegisteredCount >= Course.NumberOfSeats && Course.CourseRegistrationRequests.Where(x => x.Status == CourseRegistrationRequestStatus.Approved).Select(x => x.NumberOfRegisteredPeople).Sum() == Course.NumberOfSeats)
            {
                Course.IsClosed = true;
                Course.ClosedDate = DateTime.Now;
            }


            await Repository.UpdateAsync(registrationRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //await _CourseRegistrationRequestNotificationsAppService.SendNotificationForAcceptWorregistrationRequestToOwner(registrationRequest);
            //await _CourseRegistrationRequestNotificationsAppService.SendNotificationForAcceptWorregistrationRequestToAdmin(registrationRequest);
            return _mapper.Map<CourseRegistrationRequestDetailsDto>(registrationRequest);
        }


        [HttpPost, AbpAuthorize(PermissionNames.CourseRegistrationRequests_Reject)]

        public async Task<CourseRegistrationRequestDetailsDto> Reject(RejectCourseRegistrationRequestDto input)
        {
            var registrationRequest = await _CourseRegistrationRequestManager.GetEntityByIdAsTrackingAsync(input.Id);

            var Course = await _CourseManager.GetEntityByAsTrackingIdAsync(registrationRequest.CourseId);


            if (await _userManager.IsAdminSession())
                registrationRequest.Status = CourseRegistrationRequestStatus.Rejected;

            registrationRequest.RejectReason = input.RejectReason;

            Course.CourseRegistrationRequests.Where(x => x.Id == input.Id).FirstOrDefault().Status = CourseRegistrationRequestStatus.Rejected;



            await Repository.UpdateAsync(registrationRequest);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            //  await _CourseRegistrationRequestNotificationsAppService.SendNotificationForRejectCourseRegistrationRequestToOwner(registrationRequest);
            // await _CourseRegistrationRequestNotificationsAppService.SendNotificationForRejectCourseRegistrationRequestToAdmin(registrationRequest);

            return _mapper.Map<CourseRegistrationRequestDetailsDto>(registrationRequest);
        }



        [AbpAllowAnonymous]
        public override async Task<PagedResultDto<CourseRegistrationRequestLiteDto>> GetAllAsync(PagedCourseRegistrationRequestResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var attachmentsProfiles = await _attachmentManager.GetListByRefAsync(result.Items.Where(x => x.User.ProfileId.HasValue).Select(x => (long)x.User.ProfileId.Value).ToList(), AttachmentRefType.Profile);

            var attachmentsProfilesDict = new Dictionary<long, List<Attachment>>();

            if (attachmentsProfiles.Count > 0)
                attachmentsProfilesDict = attachmentsProfiles.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            var attachmentsCompanies = await _attachmentManager.GetListByRefAsync(result.Items.Where(x => x.User.CompanyId.HasValue).Select(x => (long)x.User.ProfileId.Value).ToList(), AttachmentRefType.CompanyLogo);

            var attachmentsCompaniesDict = new Dictionary<long, List<Attachment>>();

            if (attachmentsCompanies.Count > 0)
                attachmentsCompaniesDict = attachmentsCompanies.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (item.User.ProfileId.HasValue && attachmentsProfilesDict.TryGetValue(item.User.ProfileId.Value, out var itemProfileAttachments))
                {
                    item.User.Profile.Image = itemProfileAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }
                if (item.User.CompanyId.HasValue && attachmentsCompaniesDict.TryGetValue(item.User.CompanyId.Value, out var itemCompanyAttachments))
                {
                    item.User.Company.Profile = itemCompanyAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }

            }
            return result;
        }

        protected override IQueryable<CourseRegistrationRequest> CreateFilteredQuery(PagedCourseRegistrationRequestResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);


            data = data.Include(x => x.User).ThenInclude(x => x.Profile);
            data = data.Include(x => x.User).ThenInclude(x => x.Company).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Course).ThenInclude(x => x.Translations);

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                var keyword = input.Keyword.ToLower();

                var matchingStatus = Enum.GetValues<CourseRegistrationRequestStatus>()
                                                     .Where(e => e.ToString().ToLower().Contains(keyword))
                                                     .ToList();
                data = data.Where(crr =>

                    crr.RejectReason.Contains(input.Keyword) ||
                    (matchingStatus.Contains(crr.Status.Value) && crr.Status.HasValue) ||
                    crr.Course.Translations.Any(t => t.Title.Contains(input.Keyword)) ||
                    crr.User.Name.Contains(input.Keyword) ||
                    crr.User.Surname.Contains(input.Keyword)

                );
            }
            if (input.IsSpecial.HasValue)
            {
                if (input.IsSpecial.Value)
                    data = data.Where(crr => crr.IsSpecial == input.IsSpecial.Value);
            }

            if (!input.IsSpecial.HasValue)
                data = data.Where(crr => !crr.IsSpecial);

            if (input.CourseId.HasValue)
                data = data.Where(crr => crr.CourseId == input.CourseId.Value);

            if (input.Status.HasValue)
                data = data.Where(crr => crr.Status == input.Status.Value);
            /*
                        if (input.TraineeId.HasValue)
                            data = data.Where(crr => crr.TraineeId == input.TraineeId.Value);*/

            if (input.UserId.HasValue)
                data = data.Where(crr => crr.UserId == input.UserId.Value);

            return data;
        }
        protected override IQueryable<CourseRegistrationRequest> ApplySorting(IQueryable<CourseRegistrationRequest> query, PagedCourseRegistrationRequestResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
