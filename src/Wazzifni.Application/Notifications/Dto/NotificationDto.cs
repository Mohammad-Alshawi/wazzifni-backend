using Abp.Notifications;
using System;
using System.Collections.Generic;
using Wazzifni.NotificationService;

namespace Wazzifni.Notifications.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; set; }

        public string NotificationName { get; set; }

        public NotificationType Type { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        /// <summary>
        /// Id of related entity if exists. Depending on Type of the notification:
        /// </summary>
        //public long? RelatedId { get; set; }

        /// <summary>
        /// State: 0- Unread, 1- Read
        /// </summary>
        public UserNotificationState State { get; set; }
        public Dictionary<string, object> DataForNotification { get; set; }
    }
}