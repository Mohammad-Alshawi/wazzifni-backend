using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Skills.Dto;

namespace Wazzifni.Skills
{
    public interface ISkillAppService : IWazzifniAsyncCrudAppService<SkillDetailsDto, int, LiteSkillDto, PagedSkillResultRequestDto,
        CreateSkillDto, UpdateSkillDto>
    {
        Task<SkillDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input);

    }
}
