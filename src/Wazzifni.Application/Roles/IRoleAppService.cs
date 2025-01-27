using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Wazzifni.Roles.Dto;

namespace Wazzifni.Roles
{
    public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
    {
        Task<ListResultDto<PermissionDto>> GetAllPermissions(string? Keyword);

        Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);

        Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);
    }
}
