using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.PushNotifications.Dto
{
    public class CreatePushNotificationDto
    {
        public List<PushNotificationTranslationDto> Translations { get; set; }

        public TopicType Destination { get; set; }


    }
    public class PushNotificationWitheExcleDto
    {

        public IFormFile File { get; set; }
        public string arMessage { get; set; }
        public string enMessage { get; set; }
        public TopicType Destination { get; set; }


    }
}
