using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.WorkApplications
{
    public interface IWorkApplicationManager : IDomainService
    {

        Task<WorkApplication> GetEntityByIdAsync(long workApplicationId);

        Task<WorkApplication> GetEntityByIdAsTrackingAsync(long workApplicationId);



    }
}
