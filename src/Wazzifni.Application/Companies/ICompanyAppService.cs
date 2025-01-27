using Wazzifni.Companies.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Companies.Dto;

namespace Wazzifni.Companies
{
    public interface ICompanyAppService : IWazzifniAsyncCrudAppService<CompanyDetailsDto, int, LiteCompanyDto, PagedCompanyResultRequestDto,
        CreateCompanyDto, UpdateCompanyDto>
    {

    }
}
