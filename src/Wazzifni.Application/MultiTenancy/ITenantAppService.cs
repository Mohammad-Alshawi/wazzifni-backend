using Abp.Application.Services;
using Wazzifni.MultiTenancy.Dto;

namespace Wazzifni.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

