using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Skills
{
    public class SkillManager : DomainService, ISkillManager
    {
        private readonly IRepository<Skill> _repository;

        public SkillManager(IRepository<Skill> repository)
        {
            _repository = repository;
        }

        public async Task<Skill> GetEntityByIdAsync(long skillId)
        {
            return await _repository
                .GetAllIncluding(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == skillId).FirstOrDefaultAsync();
        }
    }
}
