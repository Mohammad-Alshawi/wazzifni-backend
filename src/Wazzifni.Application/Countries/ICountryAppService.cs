using Wazzifni.Countries.Dto;
using Wazzifni.CrudAppServiceBase;

namespace Wazzifni.Countries
{
    public interface ICountryAppService : IWazzifniAsyncCrudAppService<CountryDetailsDto, int, CountryDto, PagedCountryResultRequestDto,
        CreateCountryDto, UpdateCountryDto>
    {


    }
}
