using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.PushNotifications
{
    public interface IPushNotificationManager : IDomainService
    {
        Task<PushNotification> GetPushNotificationById(int pushNotificationId);
    }
}
