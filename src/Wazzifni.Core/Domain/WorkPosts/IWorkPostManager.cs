using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.WorkPosts
{
    public interface IWorkPostManager : IDomainService
    {

        Task<WorkPost> GetEntityByIdAsync(long workPostId);

        Task<WorkPost> GetEntityByIdAsTrackingAsync(long workPostId);

        Task<WorkPost> GetEntityWithoutUserByIdAsync(long workPostId);
    }
}
