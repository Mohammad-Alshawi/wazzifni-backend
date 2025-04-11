using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Universities.Dto;

namespace Wazzifni.Universitys
{
    public interface IUniversityAppService : IWazzifniAsyncCrudAppService<UniversityDetailsDto, int, LiteUniversityDto, PagedUniversityResultRequestDto,
        CreateUniversityDto, UpdateUniversityDto>
    {
        Task<UniversityDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input);

    }
}
