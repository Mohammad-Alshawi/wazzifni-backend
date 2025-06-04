using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Wazzifni.SignalR.NotificationHubs.Hubs
{
    public class SignalRNotificationHub : Hub
    {
        private readonly ILogger<SignalRNotificationHub> _logger;

        public SignalRNotificationHub(ILogger<SignalRNotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User connected: {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync");
            }

            await base.OnConnectedAsync(); // ⬅️ دائماً يجب أن يُستدعى هذا
        }
    }
}
