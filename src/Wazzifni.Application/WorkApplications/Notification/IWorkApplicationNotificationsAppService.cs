using Abp.Application.Services;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkApplications;


namespace Wazzifni.WorkApplicationService.Notifications;

public interface IWorkApplicationNotificationsAppService : IApplicationService
{
    Task SendNotificationForAcceptWorApplicationToOwner(WorkApplication workApplication);
    Task SendNotificationForRejectWorkApplicationToOwner(WorkApplication workApplication);
    Task SendNotificationForNewWorkApplication(WorkApplication applicant);

}