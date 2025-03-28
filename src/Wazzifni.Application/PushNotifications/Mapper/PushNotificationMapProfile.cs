using AutoMapper;
using Wazzifni.Domain.PushNotifications;
using Wazzifni.PushNotifications.Dto;

namespace Wazzifni.PushNotifications.Mapper
{
    public class PushNotificationMapProfile : Profile
    {
        public PushNotificationMapProfile()
        {

            CreateMap<CreatePushNotificationDto, PushNotificationDto>();
            CreateMap<PushNotificationDto, PushNotification>();
            CreateMap<PushNotification, PushNotificationDto>();
            CreateMap<PushNotification, LitePushNotificationDto>();
            CreateMap<PushNotification, PushNotificationDetailsDto>();
            CreateMap<PushNotification, UpdatePushNotificationDto>();
            CreateMap<CreatePushNotificationDto, PushNotification>();
            CreateMap<PushNotification, CreatePushNotificationDto>();
        }
    }
}
