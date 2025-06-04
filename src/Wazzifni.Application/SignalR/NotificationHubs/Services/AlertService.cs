using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.AspNetCore.SignalR;
using Wazzifni.Authorization.Users;
using Wazzifni.SignalR.NotificationHubs.Dto;
using Wazzifni.SignalR.NotificationHubs.Hubs;

namespace Wazzifni.SignalR.NotificationHubs.Services
{
    public class AlertService : IAlertService
    {
        private readonly IHubContext<SignalRNotificationHub> _hubContext;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly UserManager _userManager;
        private readonly string _clientsidemethodnameName = "ReceiveMessage";
        public AlertService(IHubContext<SignalRNotificationHub> hubContext, INotificationPublisher notificationPublisher, UserManager userManager)
        {
            _hubContext = hubContext;
            _notificationPublisher = notificationPublisher;
            _userManager = userManager;
        }

        public async Task SendToAllAsync(AlertMessageDto alert)
        {
            long[] UserIds = _userManager.Users.Where(x => !x.IsDeleted).Select(x => x.Id).ToArray();

            var userIdentifiers = UserIds.Select(x =>
            new UserIdentifier(MultiTenancyConsts.DefaultTenantId, x)).ToArray();

            var notificationName = alert.Type.ToString();

            await _notificationPublisher.PublishAsync(notificationName, alert, userIds: userIdentifiers);

            await _hubContext.Clients.All.SendAsync(_clientsidemethodnameName, alert);
        }

        public async Task SendToUser(long userId, AlertMessageDto alert)
        {
            List<long> userIds = new List<long>() { userId };

            var userIdentifiers = userIds.Select(x =>
            new UserIdentifier(MultiTenancyConsts.DefaultTenantId, x)).ToArray();

            var notificationName = alert.Type.ToString();

            await _notificationPublisher.PublishAsync(notificationName, alert, userIds: userIdentifiers);

            await _hubContext.Clients.User(userId.ToString()).SendAsync(_clientsidemethodnameName, userId, alert);

        }

        public async Task SendToUsers(List<long> userIds, AlertMessageDto alert)
        {
            var userIdentifiers = userIds
                .Select(x => new UserIdentifier(MultiTenancyConsts.DefaultTenantId, x))
                .ToArray();

            var notificationName = alert.Type.ToString();

            await _notificationPublisher.PublishAsync(notificationName, alert, userIds: userIdentifiers);

            var sendTasks = userIds.Select(id =>
                _hubContext.Clients.User(id.ToString()).SendAsync(_clientsidemethodnameName, id, alert)
            );

            await Task.WhenAll(sendTasks);
        }

    }

}
