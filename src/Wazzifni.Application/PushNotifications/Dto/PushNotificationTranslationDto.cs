using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.PushNotifications;

namespace Wazzifni.PushNotifications.Dto
{
    [AutoMap(typeof(PushNotificationTranslation))]
    public class PushNotificationTranslationDto
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public string Language { get; set; }
    }
}
