
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
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.CourseRegistrationRequests;
using Wazzifni.Domain.Courses;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.CourseRegistrationRequests.Dto;
using static Wazzifni.Enums.Enum;
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Wazzifni.Domain.Trainees;
using Wazzifni.Authorization;

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
            var currentTraineeId = await _TraineeManager.GetTraineeIdByUserId(AbpSession.UserId.Value);
            var Course = await _CourseManager.GetLiteCourseByIdAsync(input.CourseId);

            if(!input.IsSpecial) registrationRequest.Status = CourseRegistrationRequestStatus.Checking;
            registrationRequest.IsSpecial = input.IsSpecial;

            registrationRequest.TraineeId = currentTraineeId;


            /*if (Course.ApplicantsCount >= Course.RequiredEmployeesCount)
                Course.WorkAvailbility = WorkAvailbility.Unavilable;*/
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
            var currentUserId = AbpSession.UserId.Value;
            var currentTraineeId = await _TraineeManager.GetTraineeIdByUserId(currentUserId);
            var isOwner = registrationRequest.TraineeId == currentTraineeId;

            if (!(isOwner) && await _userManager.IsTrainee())
                throw new UserFriendlyException("You are not authorized to delete this registrationRequest.");

            if (registrationRequest.Status == CourseRegistrationRequestStatus.Approved)
                throw new UserFriendlyException(Exceptions.DeleteApprovedregistrationRequest);

            if (registrationRequest.Status == CourseRegistrationRequestStatus.Rejected)
                throw new UserFriendlyException(Exceptions.ObjectCantBeDelete);

            if (registrationRequest.Status == CourseRegistrationRequestStatus.Checking)
            {
                
                    Course.RegisteredTraineesCount--;
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

            if (Course.RegisteredTraineesCount >= Course.NumberOfSeats && Course.CourseRegistrationRequests.Where(x => x.Status == CourseRegistrationRequestStatus.Approved).Count() == Course.NumberOfSeats)
            {
                Course.IsClosed = true;
                Course.ClosedDate = DateTime.Now;
                Course.RegisteredTraineesCount++;
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

            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Trainee.Id).ToList(), AttachmentRefType.Trainee);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Trainee.Id, out var itemAttachments))
                {
                    item.Trainee.Image = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }


            }
            return result;
        }

        protected override IQueryable<CourseRegistrationRequest> CreateFilteredQuery(PagedCourseRegistrationRequestResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            
            data = data.Include(x => x.Trainee).ThenInclude(x => x.User);
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
                    crr.Trainee.User.Name.Contains(input.Keyword) ||
                    crr.Trainee.User.Surname.Contains(input.Keyword)

                );
            }
            if (input.IsSpecial.HasValue)
            {
                if(input.IsSpecial.Value)
                     data = data.Where(crr => crr.IsSpecial == input.IsSpecial.Value);
            }

            if (!input.IsSpecial.HasValue)
                data = data.Where(crr => !crr.IsSpecial);

            if (input.CourseId.HasValue)
                data = data.Where(crr => crr.CourseId == input.CourseId.Value);

            if (input.Status.HasValue)
                data = data.Where(crr => crr.Status == input.Status.Value);           

            if (input.TraineeId.HasValue)
                data = data.Where(crr => crr.TraineeId == input.TraineeId.Value);

            return data;
        }
        protected override IQueryable<CourseRegistrationRequest> ApplySorting(IQueryable<CourseRegistrationRequest> query, PagedCourseRegistrationRequestResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
