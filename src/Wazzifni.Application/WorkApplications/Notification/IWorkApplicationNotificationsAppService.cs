using Abp.Application.Services;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkApplications;


namespace Wazzifni.WorkApplicationService.Notifications;

public interface IWorkApplicationNotificationsAppService : IApplicationService
{
    Task SendNotificationForAcceptWorApplicationToOwner(WorkApplication workApplication);
    Task SendNotificationForRejectWorkApplicationToOwner(WorkApplication workApplication);
    Task SendNotificationForNewWorkApplicationToCompany(WorkApplication applicant);
    Task SendNotificationForSendWorkApplicationToOwner(WorkApplication applicant);

    Task SendNotificationForNewWorkApplicationToAdmin(WorkApplication applicant);
    Task SendNotificationForAcceptWorApplicationToAdmin(WorkApplication workApplication);
    Task SendNotificationForRejectWorkApplicationToAdmin(WorkApplication workApplication);



}