using System.Threading.Tasks;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.WorkPosts.Notifications
{
    public interface IWorkPostNotificationService
    {
        Task SendNotificationForSendWorkPostToCompany(WorkPost work);
    }
}
