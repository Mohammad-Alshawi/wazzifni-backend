using Abp.Localization.Sources;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Wazzifni.NotificationService
{
    [Serializable]
    public class TypedMessageNotificationData : NotificationData
    {
        public NotificationType NotificationType { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Messages { get; set; } = new();

        public string AdditionalValue { get; set; }

        public TypedMessageNotificationData(NotificationType notificationType, Dictionary<string, string> messages, string additionalValue, string imageUrl = "")
        {
            NotificationType = notificationType;
            Messages = messages;
            AdditionalValue = additionalValue;
            ImageUrl = imageUrl;
            Properties.Add(nameof(NotificationType), NotificationType);
            Properties.Add(nameof(AdditionalValue), AdditionalValue);
            foreach (var (lang, message) in Messages)
            {
                Properties.Add($"Message_{lang}", message);
            }
        }

        public static TypedMessageNotificationData CreateCustom(NotificationType notificationType, ILocalizationSource localizationSource,
           string AdditionalValue, Dictionary<string, string> messages, string imageUrl = "")
        {
            return new TypedMessageNotificationData(notificationType, messages, AdditionalValue, imageUrl);
        }

        public static TypedMessageNotificationData Create(NotificationType notificationType, ILocalizationSource localizationSource,
              string AdditionalValue, string imageUrl = "", params object[] localizationParams)
        {
            var messages = new Dictionary<string, string>
            {
                { "ar", localizationSource.GetString(notificationType + "Text", CultureInfo.GetCultureInfo("ar")) },
                { "en", localizationSource.GetString(notificationType + "Text", CultureInfo.GetCultureInfo("en")) },
                { "fa", localizationSource.GetString(notificationType + "Text", CultureInfo.GetCultureInfo("fa")) },
                { "ku", localizationSource.GetString(notificationType + "Text", CultureInfo.GetCultureInfo("ku")) }
            };

            return new TypedMessageNotificationData(notificationType, messages, AdditionalValue, imageUrl);
        }
    }
    public enum NotificationType : byte
    {
        PushNotification = 1,
        WorkApplicationAccept = 2,
        WorkApplicationReject = 3,
        NewWorkApplication = 4,
        WorkApplicationSent = 5,
        AcceptWorkPost = 6,
        CreateWorkPost = 7

    }
}