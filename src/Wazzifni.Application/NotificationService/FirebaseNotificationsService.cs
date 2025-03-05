using Abp;
using Abp.Application.Services;
using Abp.Configuration;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Runtime.Session;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.NotificationService
{
    //[AbpAuthorize(PermissionNames.PushNotification_Create)]
    public class FirebaseNotificationService : ApplicationService
    {

        private readonly IConfiguration _configuration;
        private readonly UserManager _userManager;
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _session;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ILocalizationSource _localizationSource;
        private readonly ILogger<FirebaseNotificationService> _logger;

        public FirebaseNotificationService(
            ISettingManager settingManager,
            IAbpSession session,
            INotificationPublisher notificationPublisher,
            IConfiguration configuration,
            ILocalizationManager localizationManager,
            UserManager userManager,
            ILogger<FirebaseNotificationService> logger)
        {
            _settingManager = settingManager;
            _session = session;
            _notificationPublisher = notificationPublisher;
            _configuration = configuration;
            _userManager = userManager;
            _localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
            _logger = logger;
            InitializeFirebaseApp();
        }

        private FirebaseApp InitializeFirebaseApp()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Firebase", "firebasesettings.json");
            GoogleCredential googleCredential = GoogleCredential.FromFile(filePath);

            if (FirebaseApp.DefaultInstance == null)
            {
                return FirebaseApp.Create(new AppOptions
                {
                    Credential = googleCredential,
                    ProjectId = _configuration["GoogleCredentials:ProjectId"]
                });
            }

            return FirebaseApp.DefaultInstance;
        }

        public async Task NotifyUsersAsync(TypedMessageNotificationData data, long[] userIds, TopicType Destenation)
        {
            var userIdentifiers = userIds.Select(x =>
                new UserIdentifier(MultiTenancyConsts.DefaultTenantId, x)).ToArray();

            var notificationName = data.NotificationType.ToString();

            await _notificationPublisher.PublishAsync(notificationName, data, userIds: userIdentifiers);

            await SendNotificationToTopic(data, TopicType.All);

        }

        public async Task SendNotificationToTopic(TypedMessageNotificationData data, TopicType Destenation)
        {
            var topic = Destenation.ToString();
            var message = PrepareMessage(data, topic);
            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation($"Message sent successfully: {response}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Firebase notification.");
                throw;
            }
        }

        private Message PrepareMessage(TypedMessageNotificationData data, string topic)
        {
            var languageCode = "ar";
            var cultureInfo = languageCode == "ar" ? "ar" : "en";
            var title = _localizationSource.GetString(data.NotificationType.ToString(), new CultureInfo(cultureInfo));

            var preferredLang = data.Messages.ContainsKey(languageCode) ? languageCode : "ar";

            var messageText = data.Messages.ContainsKey(preferredLang) ? data.Messages[preferredLang] : data.Messages["ar"];


            var dataPayload = new Dictionary<string, string>
            {
                { "time", DateTime.Now.ToString("dd-MM-yyyy HH:mm", new CultureInfo("ar")) },
                { "type", ((byte)data.NotificationType).ToString() }
            };

            foreach (var property in data.Properties)
            {
                dataPayload[property.Key] = property.Value?.ToString();
            }

            return new Message
            {
                Data = dataPayload,
                Notification = new Notification
                {
                    Title = title,
                    Body = messageText
                },
                Topic = topic
            };
        }

        public async Task SubscribeToTopic(List<string> fcmTokens, TopicType Destenation)
        {
            var topic = Destenation.ToString();

            try
            {
                await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(fcmTokens, topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to topic.");
                throw;
            }
        }

        public async Task SendToJehad(string title, string body, string fcmtoken)
        {
            var dataPayload = new Dictionary<string, string>
            {
                { "time", DateTime.Now.ToString("dd-MM-yyyy HH:mm", new CultureInfo("ar")) },
                { "type", "test"}
            };
            var message = new Message
            {
                Data = dataPayload,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Token = fcmtoken
            };
            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation($"Message sent successfully: {response}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Firebase notification.");
                throw;
            }
        }



        public async Task SubscribeToAllTopic()
        {
            var fcmTokens = await _userManager.Users
                .Where(x => !string.IsNullOrWhiteSpace(x.FcmToken))
                .Select(x => x.FcmToken)
                .ToListAsync();

            const int batchSize = 500; // Set your batch size

            for (int i = 0; i < fcmTokens.Count; i += batchSize)
            {
                var batchTokens = fcmTokens.Skip(i).Take(batchSize).ToList();
                await SubscribeToTopic(batchTokens, TopicType.All);
            }
        }


    }
}
