using Abp.Application.Services.Dto;
using Abp.Notifications;
using Wazzifni.NotificationService;

namespace Wazzifni.Notifications.Dto
{
    public class PagedNotificationResultRequestDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// State: 0- Unread, 1- Read
        /// </summary>
        public UserNotificationState? State { get; set; }

        public NotificationType? Type { get; set; }
    }
}