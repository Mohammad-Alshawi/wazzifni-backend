using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Companies;
using Wazzifni.Roles.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Roles
{
    [AbpAuthorize(PermissionNames.Pages_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly ICompanyManager _companyManager;
        private readonly UserManager _userManager;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, ICompanyManager companyManager, UserManager userManager)
            : base(repository)
        {
            _roleManager = roleManager;
            _companyManager = companyManager;
            _userManager = userManager;
        }
        [AbpAuthorize(PermissionNames.Roles_Create)]

        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }
        [AbpAuthorize(PermissionNames.Roles_List)]

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var excludedRoleNames = new[]
                        {
                            StaticRoleNames.Tenants.BasicUser,
                            StaticRoleNames.Tenants.CompanyUser,
                            StaticRoleNames.Tenants.Trainee
                        };
            var rolesQuery = _roleManager
                            .Roles
                            .WhereIf(
                                !input.Permission.IsNullOrWhiteSpace(),
                                r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                            )
                            .WhereIf(
                                input.ForCreateUser.HasValue && input.ForCreateUser.Value,
                                r => !excludedRoleNames.Contains(r.Name)
                            );

            var roles = await rolesQuery.ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }
        [AbpAuthorize(PermissionNames.Roles_Update)]

        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }
        [AbpAuthorize(PermissionNames.Roles_Delete)]

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(PermissionNames.Roles_GetAllPermission)]

        public Task<ListResultDto<PermissionDto>> GetAllPermissions(string? Keyword)
        {

            var permissions = PermissionManager.GetAllPermissions();


            var result = Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
            if (!Keyword.IsNullOrWhiteSpace())
            {
                result.Result.Items = result.Result.Items.Where(x => x.Name.Contains(Keyword) ||
                 (x.DisplayName != null && x.DisplayName.Contains(Keyword))).ToList();
            }
            return result;
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        [AbpAuthorize(PermissionNames.Roles_Get)]

        public async Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
            var roleEditDto = ObjectMapper.Map<RoleEditDto>(role);

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }
        [AbpAllowAnonymous]
        public async Task<ListResultDto<RoleListDto>> GetRolesForPhoneNumberAsync(GetRolesInputDto input)
        {
            var users = await _userManager.Users.Where(x => x.PhoneNumber == input.PhoneNumber && x.DialCode == input.DialCode).ToListAsync();
            var user_roles = new List<string>();
            foreach (var user in users)
            {
                if (user.Type == Enums.Enum.UserType.BasicUser)
                    user_roles.AddRange(await _userManager.GetRolesAsync(user));
                if (user.Type == Enums.Enum.UserType.CompanyUser && await _companyManager.GetCompanyStatusByUserIdAsync(user.Id) == CompanyStatus.Approved)
                    user_roles.AddRange(await _userManager.GetRolesAsync(user));

            }


            var roles = await _roleManager
                .Roles
                .Where(

                    r => user_roles.Contains(r.Name)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAllowAnonymous]
        public async Task<ListResultDto<RoleListDto>> GetRolesCuurenAsync()
        {
            var user = await _userManager.Users.Where(x => x.Id == AbpSession.UserId.Value).Include(x=>x.Roles).FirstOrDefaultAsync();
            var user_roles = new List<string>();
        

            user_roles.AddRange(await _userManager.GetRolesAsync(user));

           
            var roles = await _roleManager
                .Roles
                .Where(

                    r => user_roles.Contains(r.Name)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }
        protected override void CheckCreatePermission()
        {
            base.CheckCreatePermission();
            CheckPermission(PermissionNames.Pages_Roles);
        }
    }
}

