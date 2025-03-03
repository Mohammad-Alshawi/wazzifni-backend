using Abp;
using Abp.MultiTenancy;
using Abp.Notifications;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.NotificationService;

public class NotificationService : INotificationService
{
    private readonly INotificationPublisher notificationPublisher;

    public NotificationService(
        INotificationPublisher notificationPublisher
)
    {
        this.notificationPublisher = notificationPublisher;
    }

    public async Task NotifyUsersAsync(TypedMessageNotificationData data, long[] userIds, bool withNotify)
    {
        var userIdentifiers = userIds.Select(x => new UserIdentifier(MultiTenancyConsts.DefaultTenantId, x)).ToArray();

        var notificationName = data.NotificationType.ToString();

        await notificationPublisher.PublishAsync(notificationName, data, userIds: userIdentifiers, targetNotifiers: new System.Type[] { typeof(FirebaseRealTimeNotifier) });
    }
}