using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Notifications;
using Castle.Core.Logging;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;

namespace Wazzifni.NotificationService;

public class FirebaseRealTimeNotifier : IRealTimeNotifier, ITransientDependency
{
    private readonly ILogger logger;
    private readonly ISettingManager settingManager;
    private readonly IRepository<User, long> userRepository;
    private readonly ILocalizationSource localizationSource;

    public FirebaseRealTimeNotifier(
        ILogger logger,
        ISettingManager settingManager,
        IRepository<User, long> userRepository,
        ILocalizationManager localizationManager
    )
    {
        this.logger = logger;
        this.settingManager = settingManager;
        this.userRepository = userRepository;
        localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
    }

    public bool UseOnlyIfRequestedAsTarget => false;

    private const string LoggerTag = "|FIREBASE NOTIFIER|";

    private static Dictionary<string, string> GetDictionary(TypedMessageNotificationData notificationData)
    {
        var properties = notificationData.Properties;

        var dictionary = new Dictionary<string, string>
        {
            { "type", ((int)notificationData.NotificationType).ToString() },
            { "time", DateTime.Now.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) },
        };

        foreach (var key in properties.Keys.Where(key => !key.IsNullOrWhiteSpace()))
        {
            dictionary[key] = properties[key]?.ToString();
        }

        return dictionary;
    }

    public async Task SendNotificationsAsync(UserNotification[] userNotifications)
    {
        logger.Info("========== starting firebase notifier ========");

        for (var i = 0; i < userNotifications.Length; i++)
        {
            var userNotification = userNotifications[i];
            if (userNotification.Notification.Data is not TypedMessageNotificationData data) continue;

            var user = await userRepository.GetAll()
                .Where(u => u.Id == userNotification.UserId)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                logger.Info($"{LoggerTag} user with id: {userNotification.UserId} cannot be found");
                continue;
            }

            if (user.FcmToken.IsNullOrEmpty())
            {
                logger.Info($"{LoggerTag} user: {user.FullName}, id: {user.Id} has no fcm token");
                continue;
            }

            var language = await settingManager.GetSettingValueForUserAsync(
                LocalizationSettingNames.DefaultLanguage,
                userNotification.TenantId,
                userNotification.UserId
            );

            var preferredLang = data.Messages.ContainsKey(language) ? language : "ar";

            var arTitle = data.Properties.ContainsKey("ArTitle") ? data.Properties["ArTitle"] : "";
            var enTitle = data.Properties.ContainsKey("EnTitle") ? data.Properties["EnTitle"] : "";

            var title = language.ToUpper().Contains("AR")
                       ? (arTitle.ToString().IsNullOrEmpty() ? localizationSource.GetString(data.NotificationType.ToString(), CultureInfo.GetCultureInfo(preferredLang)) : arTitle)
                       : (enTitle.ToString().IsNullOrEmpty() ? localizationSource.GetString(data.NotificationType.ToString(), CultureInfo.GetCultureInfo(preferredLang)) : enTitle);


            var body = data.Messages.ContainsKey(preferredLang) ? data.Messages[preferredLang] : data.Messages["ar"];

            var image = !data.ImageUrl.IsNullOrWhiteSpace() ? data.ImageUrl : null;
            try
            {
                await FirebaseMessaging.DefaultInstance.SendAsync(
                    new Message()
                    {
                        Token = user.FcmToken,
                        Data = GetDictionary(data),
                        Notification = new Notification
                        {
                            Body = body,
                            Title = title.ToString(),
                            ImageUrl = image
                        },

                    }
                );
            }
            catch (FirebaseMessagingException firebaseException)
            {
                if (firebaseException.MessagingErrorCode == MessagingErrorCode.Unregistered)
                {
                    logger.Info($"{LoggerTag} user: {user.FullName}, id: {user.Id} has invalid fcm token {user.FcmToken} and will be deleted");
                    user.FcmToken = null;
                    continue;
                }

                logger.Error($"{LoggerTag} {firebaseException.Message}", firebaseException);
                return;
            }
            catch (Exception exception)
            {
                logger.Error($"{LoggerTag} {exception.Message}", exception);
                return;
            }

            logger.Info($"{LoggerTag} user: {user.FullName}, id: {user.Id} received the push notification with template {data.NotificationType}");
        }

        logger.Info("========== ending firebase notifier ========");
    }
}