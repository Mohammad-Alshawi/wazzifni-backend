using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.PushNotifications;
using Wazzifni.FileUploadService;
using Wazzifni.NotificationService;
using Wazzifni.PushNotifications.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.PushNotifications
{

    [AbpAuthorize(PermissionNames.Pages_PushNotification)]
    public class PushNotificationAppService : WazzifniAsyncCrudAppService<PushNotification, PushNotificationDetailsDto, int, LitePushNotificationDto,
        PagedPushNotificationResultRequestDto, CreatePushNotificationDto, UpdatePushNotificationDto>,
        IPushNotificationAppService
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager _userManager;
        private readonly IPushNotificationManager _pushNotificationManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _session;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        /// <summary>
        ///  PushNotification AppService
        /// </summary>
        public PushNotificationAppService(IRepository<PushNotification> repository,
            INotificationService notificationService,
             UserManager userManager,
            IPushNotificationManager pushNotificationManager,
            ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IAbpSession session,
            FirebaseNotificationService firebaseNotificationService,
            IFileUploadService fileUploadService,
            IMapper mapper)
         : base(repository)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _pushNotificationManager = pushNotificationManager;
            _localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
            _settingManager = settingManager;
            _session = session;
            _firebaseNotificationService = firebaseNotificationService;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
        }

        [AbpAuthorize(PermissionNames.PushNotification_Create)]
        public override async Task<PushNotificationDetailsDto> CreateAsync(CreatePushNotificationDto input)
        {
            if (input.Translations == null || !input.Translations.Any())
            {
                throw new Exception("Translations cannot be null or empty.");
            }
            var pushNotification = ObjectMapper.Map<PushNotification>(input);
            pushNotification.CreationTime = DateTime.UtcNow;

            await Repository.InsertAsync(pushNotification);
            UnitOfWorkManager.Current.SaveChanges();

            var arMessage = pushNotification.Translations?.Where(x => x.Language == "ar").Select(x => x.Message).FirstOrDefault() ?? "";
            var enMessage = pushNotification.Translations?.Where(x => x.Language == "en").Select(x => x.Message).FirstOrDefault() ?? arMessage;



            var messages = new Dictionary<string, string>
                    {
                        { "ar", arMessage },
                        { "en", enMessage },
                    };

            var data = new TypedMessageNotificationData(NotificationType.PushNotification, messages, "");
            var userIds = await GetUserIdsNotificationDestination(input.Destination);
            await _firebaseNotificationService.NotifyUsersAsync(data, userIds, input.Destination);
            return MapToEntityDto(pushNotification);
        }


        [AbpAuthorize(PermissionNames.PushNotification_List)]
        public override async Task<PagedResultDto<LitePushNotificationDto>> GetAllAsync(PagedPushNotificationResultRequestDto input)
        {
            var lang = await _settingManager.GetSettingValueForUserAsync(LocalizationSettingNames.DefaultLanguage, _session.TenantId, (long)AbpSession.UserId);

            var isArabic = lang.ToUpper().Contains("AR");

            var result = await base.GetAllAsync(input);
            foreach (var item in result.Items)
            {
                //   item.DestinationText = item.Destination.ToString();
                item.DestinationText = _localizationSource.GetString(item.Destination.ToString(), isArabic ?
                          CultureInfo.GetCultureInfo("ar-SY") :
                          CultureInfo.GetCultureInfo("en"));
                item.ArTitle = _localizationSource.GetString(NotificationType.PushNotification.ToString(), CultureInfo.GetCultureInfo("ar"));
                item.EnTitle = _localizationSource.GetString(NotificationType.PushNotification.ToString(), CultureInfo.GetCultureInfo("en"));
            }
            return result;
        }

        protected override IQueryable<PushNotification> CreateFilteredQuery(PagedPushNotificationResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Where(x => !x.IsDeleted);
            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Message.Contains(input.Keyword)).Any());
            if (input.Destination is not null)
                data = data.Where(x => x.Destination == input.Destination);
            data = data.Include(x => x.Translations);
            return data;
        }
        /// <summary>
        /// Sorting Filtered Posts
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<PushNotification> ApplySorting(IQueryable<PushNotification> query, PagedPushNotificationResultRequestDto input)
        {
            return query.OrderByDescending(r => r.CreationTime.Date);
        }
        private async Task<long[]> GetUserIdsNotificationDestination(TopicType topic)
        {
            long[] UserIds = new long[] { };
            switch (topic)
            {
                case TopicType.All:
                    UserIds = _userManager.Users.Where(x => !x.IsDeleted).Select(x => x.Id).ToArray();
                    break;
                case TopicType.Admin:
                    UserIds = _userManager.Users.Where(x => x.Type == UserType.Admin).Select(x => x.Id).ToArray();
                    break;
                case TopicType.BasicUser:
                    UserIds = _userManager.Users.Where(x => x.Type == UserType.BasicUser).Select(x => x.Id).ToArray();
                    break;
                case TopicType.CompanyUser:
                    UserIds = _userManager.Users.Where(x => x.Type == UserType.CompanyUser).Select(x => x.Id).ToArray();
                    break;
            }
            return UserIds;
        }
    }
}
