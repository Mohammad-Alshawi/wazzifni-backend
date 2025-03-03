using Abp.Application.Services;
using Abp.Localization;
using Abp.Localization.Sources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.NotificationService;

namespace Wazzifni.WorkApplicationService.Notifications;

[RemoteService(IsEnabled = false)]
public class WorkApplicationNotificationsAppService : IWorkApplicationNotificationsAppService
{
    private readonly IWorkApplicationManager _workApplicationManager;
    private readonly ILocalizationSource _localizationSource;
    private readonly INotificationService _InotificationService;

    public WorkApplicationNotificationsAppService(IWorkApplicationManager workApplicationManager, ILocalizationManager localizationManager, INotificationService inotificationService)
    {
        _workApplicationManager = workApplicationManager;
        _localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
        _InotificationService = inotificationService;
    }

    public async Task SendNotificationForAcceptApplication(WorkApplication workApplication)
    {
        var messages = new Dictionary<string, string>
        {
            { "ar", $"تم نشر وظيفة جديدة: {workApplication.WorkPost.Title}" },
            { "en", $"A new job has been posted: {workApplication.WorkPost.Title}" }
        };
        var data = new TypedMessageNotificationData(NotificationType.WorkApplicationAccept, messages, "");
        data.Properties.Add("WorkApplicationId", workApplication.Id);
        data.Properties.Add("Slug", workApplication.WorkPost.Slug);
        List<long> userIds = new List<long> { workApplication.Profile.UserId };
        await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);

    }





    ////////////////////////////////////////Help Method ////////////////////////////////////////////////////

    private async Task NotifyUser(WorkApplication WorkApplication)
    {

        var (yourWorkApplicationAr, yourWorkApplicationEn) = GetYourWorkApplicationMessage(WorkApplication);

        var prefixAr = _localizationSource.GetString("ReminderPrefix", CultureInfo.GetCultureInfo("ar"));
        var prefixEn = _localizationSource.GetString("ReminderPrefix", CultureInfo.GetCultureInfo("en"));
        var endMessageAr = _localizationSource.GetString("ReminderEndMessge", CultureInfo.GetCultureInfo("ar"));
        var endMessageEn = _localizationSource.GetString("ReminderEndMessge", CultureInfo.GetCultureInfo("en"));

        string locationAr = string.Empty;
        string locationEn = string.Empty;

        var (cityAr, cityEn) = GetCityNames(WorkApplication);

        locationAr = $"{cityAr}";
        locationEn = $"{cityEn}";


        string finalArMessage = $"{prefixAr}{yourWorkApplicationAr} ({(string.IsNullOrEmpty(locationAr) ? "" : $", {locationAr}")})، {Environment.NewLine}{endMessageAr}";
        string finalEnMessage = $"{prefixEn}{yourWorkApplicationEn} ({(string.IsNullOrEmpty(locationEn) ? "" : $", {locationEn}")}), {Environment.NewLine}{endMessageEn}";

        var notificationType = NotificationType.WorkApplicationAccept;

        var data = new TypedMessageNotificationData(notificationType, finalArMessage, finalEnMessage, "");
        data.Properties.Add("WorkApplicationId", WorkApplication.Id);
        data.Properties.Add("Slug", WorkApplication.WorkPost.Slug);

        await _InotificationService.NotifyUsersAsync(data, new[] { WorkApplication.Profile.UserId }, true);
    }


    private (string, string) GetYourWorkApplicationMessage(WorkApplication workApplication)
    {
        string key = workApplication.WorkPost.Slug.StartsWith("PR") ? "ReminderYourWorkApplication" : "ReminderYourCar";
        string yourWorkApplicationAr = _localizationSource.GetString(key, CultureInfo.GetCultureInfo("ar"));
        string yourWorkApplicationEn = _localizationSource.GetString(key, CultureInfo.GetCultureInfo("en"));

        return (yourWorkApplicationAr, yourWorkApplicationEn);
    }
    private (string, string) GetCityNames(WorkApplication workApplication)
    {
        var cityAr = workApplication.WorkPost.Company.City.Translations.FirstOrDefault(x => x.Language == "ar")?.Name ?? "";
        var cityEn = workApplication.WorkPost.Company.City.Translations.FirstOrDefault(x => x.Language == "en")?.Name ?? "";

        return (cityAr, cityEn);
    }


}