using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Skills
{
    public interface ISkillManager : IDomainService
    {
        Task<Skill> GetEntityByIdAsync(long skillId);
    }
}
