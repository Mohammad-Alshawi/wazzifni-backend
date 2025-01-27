using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.Cities.Dto;
using Wazzifni.CrudAppServiceBase;

namespace Wazzifni.Cities
{
    public interface ICityAppService : IWazzifniAsyncCrudAppService<CityDetailsDto, int, LiteCityDto, PagedCityResultRequestDto,
        CreateCityDto, UpdateCityDto>
    {
        Task<CityDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input);

    }
}
