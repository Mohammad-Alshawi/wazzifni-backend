using System;
using System.Collections.Generic;
using System.Globalization;
using Abp.Localization.Sources;
using Abp.Notifications;

namespace Wazzifni.SignalR.NotificationHubs.Dto
{
    [Serializable]
    public class AlertMessageDto : NotificationData
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public Dictionary<string, object> Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                foreach (KeyValuePair<string, object> item in value)
                {
                    if (!_properties.ContainsKey(item.Key))
                    {
                        _properties[item.Key] = item.Value;
                    }
                }
            }
        }

        public Dictionary<string, string> Messages { get; set; } = new();
        public AlertType Type { get; set; }
        public string UserId { get; set; }

        public string AdditionalValue { get; set; }

        public AlertMessageDto(AlertType AlertType, Dictionary<string, string> messages, string additionalValue, string imageUrl = "")
        {
            Type = AlertType;
            Messages = messages;
            AdditionalValue = additionalValue;
            Properties.Add(nameof(AlertType), AlertType);
            Properties.Add(nameof(AdditionalValue), AdditionalValue);
            foreach (var (lang, message) in Messages)
            {
                Properties.Add($"Message_{lang}", message);
            }
        }

        public static AlertMessageDto CreateCustom(AlertType AlertType, ILocalizationSource localizationSource,
           string AdditionalValue, Dictionary<string, string> messages, string imageUrl = "")
        {
            return new AlertMessageDto(AlertType, messages, AdditionalValue, imageUrl);
        }

        public static AlertMessageDto Create(AlertType AlertType, ILocalizationSource localizationSource,
              string AdditionalValue, string imageUrl = "", params object[] localizationParams)
        {
            var messages = new Dictionary<string, string>
            {
                { "ar", localizationSource.GetString(AlertType + "Text", CultureInfo.GetCultureInfo("ar")) },
                { "en", localizationSource.GetString(AlertType + "Text", CultureInfo.GetCultureInfo("en")) },
                { "de", localizationSource.GetString(AlertType + "Text", CultureInfo.GetCultureInfo("de")) }
            };

            return new AlertMessageDto(AlertType, messages, AdditionalValue, imageUrl);
        }
    }

}
