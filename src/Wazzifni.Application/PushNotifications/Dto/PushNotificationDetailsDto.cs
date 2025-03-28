using Abp.Application.Services.Dto;

namespace Wazzifni.PushNotifications.Dto
{
    public class PushNotificationDetailsDto : EntityDto
    {
        public string Message { get; set; }
    }
}
