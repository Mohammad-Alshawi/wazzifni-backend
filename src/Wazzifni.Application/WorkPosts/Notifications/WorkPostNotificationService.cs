using Abp.Application.Services;
using Abp.Localization;
using Abp.Localization.Sources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.NotificationService;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts.Notifications
{
    [RemoteService(IsEnabled = false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WorkPostNotificationService : IWorkPostNotificationService
    {
        private readonly IWorkPostManager _workPostManager;
        private readonly UserManager _userManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly INotificationService _InotificationService;
        public WorkPostNotificationService(IWorkPostManager workPostManager, UserManager userManager, ILocalizationManager localizationManager, INotificationService inotificationService)
        {
            _workPostManager = workPostManager;
            _userManager = userManager;
            _localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
            _InotificationService = inotificationService;
        }

        public async Task SendNotificationForAcceptWorkPostToAdmin(WorkPost work)
        {
            string arCompanyName, enCompanyName, faCompanyName, kuCompanyName;
            GetCompanyNameWithLocalization(work, out arCompanyName, out enCompanyName, out faCompanyName, out kuCompanyName);

            var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkPostAcceptedToAdmin", CultureInfo.GetCultureInfo("ar"),work.Title,arCompanyName)) },
                        { "en", string.Format(_localizationSource.GetString("WorkPostAcceptedToAdmin", CultureInfo.GetCultureInfo("en"),work.Title,enCompanyName)) },
                        { "ku", string.Format(_localizationSource.GetString("WorkPostAcceptedToAdmin", CultureInfo.GetCultureInfo("ku"),work.Title,kuCompanyName)) },
                        { "fa", string.Format(_localizationSource.GetString("WorkPostAcceptedToAdmin", CultureInfo.GetCultureInfo("fa"),work.Title,faCompanyName)) }
                    };

            var data = new TypedMessageNotificationData(NotificationType.WorkPostAccept, messages, "");

            data.Properties.Add("WorkPostId", work.Id);
            data.Properties.Add("Slug", work.Slug);
            data.Properties.Add("CompanyId", work.CompanyId);

            List<long> userIds = new List<long> { };

            var adminsIds = await _userManager.Users.Where(x => x.Type == UserType.Admin).Select(x => x.Id).ToArrayAsync();
            userIds.AddRange(adminsIds);

            await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
        }

        public async Task SendNotificationForAcceptWorkPostToCompany(WorkPost work)
        {
            var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkPostAcceptedToCompany", CultureInfo.GetCultureInfo("ar"),work.Title)) },
                        { "en", string.Format(_localizationSource.GetString("WorkPostAcceptedToCompany", CultureInfo.GetCultureInfo("en"),work.Title)) },
                        { "ku", string.Format(_localizationSource.GetString("WorkPostAcceptedToCompany", CultureInfo.GetCultureInfo("ku"),work.Title)) },
                        { "fa", string.Format(_localizationSource.GetString("WorkPostAcceptedToCompany", CultureInfo.GetCultureInfo("fa"),work.Title)) }
                    };

            var data = new TypedMessageNotificationData(NotificationType.WorkPostAccept, messages, "");

            data.Properties.Add("WorkPostId", work.Id);
            data.Properties.Add("Slug", work.Slug);
            data.Properties.Add("CompanyId", work.CompanyId);

            List<long> userIds = new List<long> { work.Company.UserId.Value };
            await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
        }

        public async Task SendNotificationForCreateWorkPostToAdmin(WorkPost work)
        {
            string arCompanyName, enCompanyName, faCompanyName, kuCompanyName;
            GetCompanyNameWithLocalization(work, out arCompanyName, out enCompanyName, out faCompanyName, out kuCompanyName);

            var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkPostCreateToAdmin", CultureInfo.GetCultureInfo("ar"),arCompanyName,work.Title)) },
                        { "en", string.Format(_localizationSource.GetString("WorkPostCreateToAdmin", CultureInfo.GetCultureInfo("en"),enCompanyName,work.Title)) },
                        { "ku", string.Format(_localizationSource.GetString("WorkPostCreateToAdmin", CultureInfo.GetCultureInfo("ku"),kuCompanyName,work.Title)) },
                        { "fa", string.Format(_localizationSource.GetString("WorkPostCreateToAdmin", CultureInfo.GetCultureInfo("fa"),faCompanyName,work.Title)) }
                    };

            var data = new TypedMessageNotificationData(NotificationType.NewWorkPost, messages, "");

            data.Properties.Add("WorkPostId", work.Id);
            data.Properties.Add("Slug", work.Slug);
            data.Properties.Add("CompanyId", work.CompanyId);

            List<long> userIds = new List<long> { };
            var adminsIds = await _userManager.Users.Where(x => x.Type == UserType.Admin).Select(x => x.Id).ToArrayAsync();
            userIds.AddRange(adminsIds); await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
        }

        private static void GetCompanyNameWithLocalization(WorkPost work, out string arCompanyName, out string enCompanyName, out string faCompanyName, out string kuCompanyName)
        {
            arCompanyName = work.Company.Translations
           .Where(x => x.Language == "ar").Select(x => x.Name).FirstOrDefault() ?? work.Company.Translations
           .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault();
            enCompanyName = work.Company.Translations
                .Where(x => x.Language == "en").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;
            faCompanyName = work.Company.Translations
                .Where(x => x.Language == "fa").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;
            kuCompanyName = work.Company.Translations
                .Where(x => x.Language == "ku").Select(x => x.Name).FirstOrDefault() ?? arCompanyName;
        }

        public async Task SendNotificationForSendWorkPostToCompany(WorkPost work)
        {

            var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("ar"))) },
                        { "en", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("en"))) },
                        { "ku", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("ku"))) },
                        { "fa", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("fa"))) }
                    };

            var data = new TypedMessageNotificationData(NotificationType.WorkPostSent, messages, "");

            data.Properties.Add("WorkPostId", work.Id);
            data.Properties.Add("Slug", work.Slug);
            data.Properties.Add("CompanyId", work.CompanyId);


            List<long> userIds = new List<long> { work.Company.UserId.Value };
            await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
        }
    }
}
