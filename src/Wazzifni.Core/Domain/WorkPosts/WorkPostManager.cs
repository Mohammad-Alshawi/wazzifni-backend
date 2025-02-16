using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.Domain.WorkPosts
{
    public class WorkPostManager : DomainService, IWorkPostManager
    {
        private readonly IRepository<WorkPost, long> _repository;

        public WorkPostManager(IRepository<WorkPost, long> repository)
        {
            _repository = repository;
        }


        public async Task<WorkPost> GetEntityByIdAsync(long workPostId)
        {
            return await _repository
                .GetAll().Include(x => x.Company).ThenInclude(x => x.Translations)
                .Include(x => x.Company).ThenInclude(x => x.User)
                .AsNoTracking().Where(x => x.Id == workPostId).FirstOrDefaultAsync();
        }
        public async Task<WorkPost> GetEntityByIdAsTrackingAsync(long workPostId)
        {
            return await _repository
                .GetAll().Include(x => x.Company).ThenInclude(x => x.Translations)
                .Include(x => x.Company).ThenInclude(x => x.User)
                .Where(x => x.Id == workPostId).FirstOrDefaultAsync();
        }
    }
}
