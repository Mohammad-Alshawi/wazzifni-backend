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
                .GetAllIncluding(x => x.Company)
                .AsNoTracking().Where(x => x.Id == workPostId).FirstOrDefaultAsync();
        }
    }
}
