using Abp.Localization;
using Abp.Localization.Sources;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.NotificationService;

namespace Wazzifni.WorkPosts.Notifications
{
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


        public async Task SendNotificationForSendWorkPostToCompany(WorkPost work)
        {

            var messages = new Dictionary<string, string>
                    {
                        { "ar", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("ar"))) },
                        { "en", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("en"))) },
                        { "ku", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("ku"))) },
                        { "fa", string.Format(_localizationSource.GetString("WorkPostSentForCompany", CultureInfo.GetCultureInfo("fa"))) }
                    };

            var data = new TypedMessageNotificationData(NotificationType.WorkApplicationSent, messages, "");

            data.Properties.Add("WorkPostId", work.Id);
            data.Properties.Add("Slug", work.Slug);

            List<long> userIds = new List<long> { work.Company.UserId.Value };
            await _InotificationService.NotifyUsersAsync(data, userIds.ToArray(), true);
        }
    }
}
