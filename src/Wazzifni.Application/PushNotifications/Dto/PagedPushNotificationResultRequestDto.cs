using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.PushNotifications.Dto
{
    public class PagedPushNotificationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }



        public TopicType? Destination { get; set; }

    }
}
