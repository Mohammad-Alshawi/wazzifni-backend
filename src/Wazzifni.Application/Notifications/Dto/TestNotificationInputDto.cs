using Wazzifni.NotificationService;

namespace Wazzifni.Notifications.Dto
{
    public class TestNotificationInputDto
    {
        //  public long? RelatedId { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Type of Notification:
        /// 1 - 10
        /// </summary>
        public NotificationType Type { get; set; }
        public long[] UserIds { get; set; }
    }
}
