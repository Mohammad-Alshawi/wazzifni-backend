using Wazzifni.CrudAppServiceBase;
using Wazzifni.PushNotifications.Dto;

namespace Wazzifni.PushNotifications
{
    public interface IPushNotificationAppService : IWazzifniAsyncCrudAppService<PushNotificationDetailsDto, int, LitePushNotificationDto, PagedPushNotificationResultRequestDto,
         CreatePushNotificationDto, UpdatePushNotificationDto>
    {
    }
}
