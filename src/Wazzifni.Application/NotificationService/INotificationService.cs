using Abp.Dependency;
using System.Threading.Tasks;

namespace Wazzifni.NotificationService;

public interface INotificationService : ITransientDependency
{
    Task NotifyUsersAsync(TypedMessageNotificationData data, long[] userIds, bool withNotify);
}