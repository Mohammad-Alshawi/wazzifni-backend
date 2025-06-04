using System.Threading.Tasks;
using Abp.Dependency;
using Wazzifni.SignalR.NotificationHubs.Dto;

namespace Wazzifni.SignalR.NotificationHubs.Services
{
    public interface IAlertService : ITransientDependency
    {

        Task SendToAllAsync(AlertMessageDto alert);
        Task SendToUser(long userId, AlertMessageDto alert);

    }

}
