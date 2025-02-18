using Abp.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.Domain.WorkApplications
{
    public interface IWorkApplicationManager : IDomainService
    {

        Task<WorkApplication> GetEntityByIdAsync(long workApplicationId);

        Task<WorkApplication> GetEntityByIdAsTrackingAsync(long workApplicationId);

        Task<HashSet<long>> GetUserAppliedWorkPostIdsAsync(long userId, List<long> workPostIds);

        IQueryable<WorkPost> GetApplyWorkPostsQueryByUserIdAsync(long userId);

    }
}
