using Abp.Application.Services;
using Abp.Localization;
using Abp.Localization.Sources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.NotificationService;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkApplicationService.Notifications;

[RemoteService(IsEnabled = false)]
public class WorkApplicationNotificationsAppService : IWorkApplicationNotificationsAppService
{
    private readonly IWorkApplicationManager _workApplicationManager;
    private readonly UserManager _userManager;
    private readonly ILocalizationSource _localizationSource;
    private readonly INotificationService _InotificationService;

    public WorkApplicationNotificationsAppService(IWorkApplicationManager workApplicationManager, UserManager userManager, ILocalizationManager localizationManager, INotificationService inotificationService)
    {
        _workApplicationManager = workApplicationManager;
        _userManager = userManager;
        _localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
        _InotificationService = inotificationService;
    }

    public async Task SendNotificationForAcceptWorApplicationToOwner(WorkApplication workApplication)
    {
        var arCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "ar").Select(x => x.Name).FirstOrDefault() ?? workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault();

        var enCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var faCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "fa").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var kuCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "ku").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var messages = new Dictionary<string, string>
        {
            { "ar", string.Format(_localizationSource.GetString("WorkApplicationAcceptedToOwner", CultureInfo.GetCultureInfo("ar")),workApplication.Profile.User.RegistrationFullName,arCompanyName) },
            { "en", string.Format(_localizationSource.GetString("WorkApplicationAcceptedToOwner", CultureInfo.GetCultureInfo("en")),workApplication.Profile.User.RegistrationFullName,enCompanyName) },
            { "ku", string.Format(_localizationSource.GetString("WorkApplicationAcceptedToOwner", CultureInfo.GetCultureInfo("ku")),workApplication.Profile.User.RegistrationFullName,kuCompanyName) },
            { "fa", string.Format(_localizationSource.GetString("WorkApplicationAcceptedToOwner", CultureInfo.GetCultureInfo("fa")),workApplication.Profile.User.RegistrationFullName,faCompanyName) }

        };
        var data = new TypedMessageNotificationData(NotificationType.WorkApplicationAccept, messages, "");
        data.Properties.Add("WorkApplicationId", workApplication.Id);
        data.Properties.Add("Slug", workApplication.WorkPost.Slug);
        List<long> userIds = new List<long> { workApplication.Profile.UserId };
        await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
    }

    public async Task SendNotificationForRejectWorkApplicationToOwner(WorkApplication workApplication)
    {
        var arCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "ar").Select(x => x.Name).FirstOrDefault() ?? workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault();

        var enCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var faCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "fa").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var kuCompanyName = workApplication.WorkPost.Company.Translations
            .Where(x => x.Language == "ku").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkApplicationRejectedToOwner", CultureInfo.GetCultureInfo("ar")), workApplication.Profile.User.RegistrationFullName, arCompanyName, workApplication.RejectReason) },
                        { "en", string.Format(_localizationSource.GetString("WorkApplicationRejectedToOwner", CultureInfo.GetCultureInfo("en")), workApplication.Profile.User.RegistrationFullName, enCompanyName, workApplication.RejectReason) },
                        { "ku", string.Format(_localizationSource.GetString("WorkApplicationRejectedToOwner", CultureInfo.GetCultureInfo("ku")), workApplication.Profile.User.RegistrationFullName, kuCompanyName, workApplication.RejectReason) },
                        { "fa", string.Format(_localizationSource.GetString("WorkApplicationRejectedToOwner", CultureInfo.GetCultureInfo("fa")), workApplication.Profile.User.RegistrationFullName, faCompanyName, workApplication.RejectReason) }
                    };

        var data = new TypedMessageNotificationData(NotificationType.WorkApplicationReject, messages, "");
        data.Properties.Add("WorkApplicationId", workApplication.Id);
        data.Properties.Add("Slug", workApplication.WorkPost.Slug);

        List<long> userIds = new List<long> { workApplication.Profile.UserId };
        await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
    }



    public async Task SendNotificationForNewWorkApplication(WorkApplication applicant)
    {
        var arCompanyName = applicant.WorkPost.Company.Translations
            .Where(x => x.Language == "ar").Select(x => x.Name).FirstOrDefault() ?? applicant.WorkPost.Company.Translations
            .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault();

        var enCompanyName = applicant.WorkPost.Company.Translations
            .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var faCompanyName = applicant.WorkPost.Company.Translations
            .Where(x => x.Language == "fa").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var kuCompanyName = applicant.WorkPost.Company.Translations
            .Where(x => x.Language == "ku").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;

        var jobTitle = applicant.WorkPost.Title;



        var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("NewWorkApplicationSubmitted", CultureInfo.GetCultureInfo("ar")), applicant.Profile.User.RegistrationFullName, jobTitle, arCompanyName) },
                        { "en", string.Format(_localizationSource.GetString("NewWorkApplicationSubmitted", CultureInfo.GetCultureInfo("en")), applicant.Profile.User.RegistrationFullName, jobTitle, enCompanyName) },
                        { "ku", string.Format(_localizationSource.GetString("NewWorkApplicationSubmitted", CultureInfo.GetCultureInfo("ku")), applicant.Profile.User.RegistrationFullName, jobTitle, kuCompanyName) },
                        { "fa", string.Format(_localizationSource.GetString("NewWorkApplicationSubmitted", CultureInfo.GetCultureInfo("fa")), applicant.Profile.User.RegistrationFullName, jobTitle, faCompanyName) }
                    };

        var data = new TypedMessageNotificationData(NotificationType.NewWorkApplication, messages, "");

        data.Properties.Add("WorkApplicationId", applicant.Id);
        data.Properties.Add("WorkPostId", applicant.WorkPostId);
        data.Properties.Add("Slug", applicant.WorkPost.Slug);

        List<long> userIds = new List<long> { applicant.WorkPost.Company.UserId.Value };
        var admins = await _userManager.Users.Where(x => x.Type == UserType.Admin).Select(x => x.Id).ToArrayAsync();
        userIds.AddRange(admins);
        await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
    }



    public async Task SendNotificationForSendWorkApplicationToOwner(WorkApplication applicant)
    {

        var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkApplicationSentForOwner", CultureInfo.GetCultureInfo("ar"))) },
                        { "en", string.Format(_localizationSource.GetString("WorkApplicationSentForOwner", CultureInfo.GetCultureInfo("en"))) },
                        { "ku", string.Format(_localizationSource.GetString("WorkApplicationSentForOwner", CultureInfo.GetCultureInfo("ku"))) },
                        { "fa", string.Format(_localizationSource.GetString("WorkApplicationSentForOwner", CultureInfo.GetCultureInfo("fa"))) }
                    };

        var data = new TypedMessageNotificationData(NotificationType.WorkApplicationSent, messages, "");

        data.Properties.Add("WorkApplicationId", applicant.Id);
        data.Properties.Add("WorkPostId", applicant.WorkPostId);
        data.Properties.Add("Slug", applicant.WorkPost.Slug);

        List<long> userIds = new List<long> { applicant.Profile.UserId };
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
        var messages = new Dictionary<string, string>
        {


        };
        var data = new TypedMessageNotificationData(notificationType, messages, "");
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