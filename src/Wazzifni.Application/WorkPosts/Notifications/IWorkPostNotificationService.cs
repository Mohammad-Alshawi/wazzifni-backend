using System.Threading.Tasks;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.WorkPosts.Notifications
{
    public interface IWorkPostNotificationService
    {
        Task SendNotificationForSendWorkPostToCompany(WorkPost work);

        Task SendNotificationForCreateWorkPostToAdmin(WorkPost work);
        Task SendNotificationForAcceptWorkPostToCompany(WorkPost work);
        Task SendNotificationForAcceptWorkPostToAdmin(WorkPost work);

    }
}
