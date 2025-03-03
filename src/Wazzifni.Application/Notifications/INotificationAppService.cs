using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;
using Wazzifni.Notifications.Dto;

namespace Wazzifni.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        Task<PagedResultDto<NotificationDto>> GetUserNotificationsAsync(PagedNotificationResultRequestDto input);
        Task MarkNotificationAsReadAsync(EntityDto<Guid> input);
        Task<int> GetNumberOfUnReadUserNotificationsAsync();
    }
}