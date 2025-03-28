using Abp.Application.Services.Dto;

namespace Wazzifni.PushNotifications.Dto
{
    public class PushNotificationDto : EntityDto<int>
    {
        public string Message { get; set; }
    }
}
