using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.Domain.WorkApplications
{
    public class WorkApplicationManager : DomainService, IWorkApplicationManager
    {
        private readonly IRepository<WorkApplication, long> _repository;

        public WorkApplicationManager(IRepository<WorkApplication, long> repository)
        {
            _repository = repository;
        }


        public async Task<WorkApplication> GetEntityByIdAsync(long workApplicationId)
        {
            return await _repository
                .GetAll().Include(x => x.Profile).ThenInclude(x => x.User)
                .Include(x => x.WorkPost).ThenInclude(x => x.Company).ThenInclude(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == workApplicationId).FirstOrDefaultAsync();
        }

        public async Task<WorkApplication> GetEntityByIdAsTrackingAsync(long workApplicationId)
        {
            return await _repository
                .GetAll().Include(x => x.Profile).ThenInclude(x => x.User)
                .Include(x => x.WorkPost).ThenInclude(x => x.Company).ThenInclude(x => x.Translations)
                .Where(x => x.Id == workApplicationId).FirstOrDefaultAsync();
        }
    }
}
