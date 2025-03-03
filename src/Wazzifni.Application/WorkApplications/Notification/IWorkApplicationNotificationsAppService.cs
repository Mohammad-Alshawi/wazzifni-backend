using Abp.Application.Services;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkApplications;


namespace Wazzifni.WorkApplicationService.Notifications;

public interface IWorkApplicationNotificationsAppService : IApplicationService
{
    Task SendNotificationForAcceptApplication(WorkApplication workApplication);

}