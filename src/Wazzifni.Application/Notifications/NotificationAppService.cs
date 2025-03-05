using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Notifications;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Notifications.Dto;
using Wazzifni.NotificationService;

namespace Wazzifni.Notifications
{
    [AbpAuthorize]
    public partial class NotificationAppService : WazzifniAppServiceBase, INotificationAppService
    {
        private readonly INotificationService _notificationsService;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly IAbpSession _session;
        private readonly UserManager _userManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public NotificationAppService(
            INotificationService notificationsService,
            IUserNotificationManager userNotificationManager,
            IAbpSession session,
             UserManager userManager,
             IUnitOfWorkManager unitOfWorkManager)
        {
            _notificationsService = notificationsService;
            _userNotificationManager = userNotificationManager;
            _session = session;
            _userManager = userManager;
            _unitOfWorkManager = unitOfWorkManager;

        }


        public async Task<PagedResultDto<NotificationDto>> GetUserNotificationsAsync(PagedNotificationResultRequestDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

                try
                {
                    var userIdentifier = new UserIdentifier(AbpSession.GetTenantId(), AbpSession.GetUserId());
                    var userNotifications = await _userNotificationManager.GetUserNotificationsAsync(userIdentifier, input.State, input.SkipCount, input.MaxResultCount);
                    var totalCount = await _userNotificationManager.GetUserNotificationCountAsync(userIdentifier, input.State);

                    var result = new List<NotificationDto>();

                    var lang = await SettingManager.GetSettingValueForUserAsync(
                        LocalizationSettingNames.DefaultLanguage,
                        userIdentifier);


                    foreach (var item in userNotifications)
                    {
                        var data = item.Notification.Data.As<TypedMessageNotificationData>();

                        var preferredLang = data.Messages.ContainsKey(lang) ? lang : "ar";

                        result.Add(new NotificationDto
                        {
                            Id = item.Id,
                            NotificationName = L(item.Notification.NotificationName),
                            Type = data.NotificationType,
                            Message = data.Messages.ContainsKey(preferredLang) ? data.Messages[preferredLang] : data.Messages["ar"],
                            DateTime = item.Notification.CreationTime,
                            DataForNotification = item.Notification.Data.Properties,
                            State = item.State,
                        });
                    }
                    if (input.Type.HasValue)
                    {
                        result = result.Where(x => x.Type == input.Type.Value).ToList();
                    }
                    return new PagedResultDto<NotificationDto>
                    {
                        TotalCount = totalCount,
                        Items = result
                    };
                }
                catch (Exception e)
                {

                }
                return new PagedResultDto<NotificationDto>
                {
                    TotalCount = 1,
                    Items = new List<NotificationDto>()
                };
            }


        }

        public async Task<int> GetNumberOfUnReadUserNotificationsAsync()
        {
            var userIdentifier = new UserIdentifier(AbpSession.GetTenantId(), AbpSession.GetUserId());
            return await _userNotificationManager.GetUserNotificationCountAsync(userIdentifier, UserNotificationState.Unread);
        }


        public async Task MarkNotificationAsReadAsync(EntityDto<Guid> input)
        {
            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.GetTenantId(), input.Id, UserNotificationState.Read);
        }

    }
}
