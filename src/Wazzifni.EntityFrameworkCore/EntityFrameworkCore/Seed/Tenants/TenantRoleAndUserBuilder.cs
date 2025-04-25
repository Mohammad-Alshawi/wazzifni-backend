using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Roles;

namespace Wazzifni.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly WazzifniDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(WazzifniDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var basicUserRole = _context.Roles.IgnoreQueryFilters().Include(x => x.Permissions).FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.BasicUser);
            if (basicUserRole == null)
            {
                basicUserRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.BasicUser, StaticRoleNames.Tenants.BasicUser) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var CompanyUserRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.CompanyUser);
            if (CompanyUserRole == null)
            {
                CompanyUserRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.CompanyUser, StaticRoleNames.Tenants.CompanyUser) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var TraineeUserRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Trainee);
            if (TraineeUserRole == null)
            {
                TraineeUserRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Trainee, StaticRoleNames.Tenants.Trainee) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }


            CheckAdminRoles(adminRole);

            CheckBasicUserRoles(basicUserRole);
            CheckCompanyUserRoles(CompanyUserRole);
            CheckTraineeUserRoles(TraineeUserRole);


        }
        private void CheckBasicUserRoles(Role basicUserRole)
        {
            var basicUserPermissionInDB = _context
                .Permissions
                .IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == basicUserRole.Id)
                .Select(x => x.Name)
                .ToList();

            var allBasicUserPermissions = new List<string>
            {
                PermissionNames.Pages_Users,

                 PermissionNames.WorkApplications,
                 PermissionNames.WorkApplications_Create,
                 PermissionNames.WorkApplications_Update,
                 PermissionNames.WorkApplications_Delete,

            };

            GrantPermissionToRole(
                role: basicUserRole,
                alreadyIncludedPermissions: basicUserPermissionInDB,
                actualPermissions: allBasicUserPermissions
            );
        }

        private void CheckCompanyUserRoles(Role CompanyUserRole)
        {
            var CompanyUserPermissionInDB = _context
                .Permissions
                .IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == CompanyUserRole.Id)
                .Select(x => x.Name)
                .ToList();

            var allCompanyUserPermissions = new List<string>
            {
                PermissionNames.Pages_Users,

                  PermissionNames.Pages_Users,
                  PermissionNames.Companies,
                  PermissionNames.Companies_Create,
                  PermissionNames.Companies_Update,
                  PermissionNames.Companies_Read,
                  PermissionNames.WorkPosts,
                  PermissionNames.WorkPosts_Create,
                  PermissionNames.WorkPosts_Update,
                  PermissionNames.WorkPosts_Read,
                  PermissionNames.WorkApplications,
                  PermissionNames.WorkApplications_Approve,
                  PermissionNames.WorkApplications_Reject,
                  PermissionNames.WorkApplications_Delete,



            };

            GrantPermissionToRole(
                role: CompanyUserRole,
                alreadyIncludedPermissions: CompanyUserPermissionInDB,
                actualPermissions: allCompanyUserPermissions
            );
        }


        private void CheckTraineeUserRoles(Role traineeUserRole)
        {
            var traineeUserPermissionInDB = _context
                .Permissions
                .IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == traineeUserRole.Id)
                .Select(x => x.Name)
                .ToList();

            var allTraineeUserPermissions = new List<string>
            {
                PermissionNames.Pages_Users,

                 PermissionNames.CourseRegistrationRequests,
                 PermissionNames.CourseRegistrationRequests_Create,
                 PermissionNames.CourseRegistrationRequests_Update,
                 PermissionNames.CourseRegistrationRequests_Delete,
                 PermissionNames.Courses,
                 PermissionNames.Courses_Rate

            };

            GrantPermissionToRole(
                role: traineeUserRole,
                alreadyIncludedPermissions: traineeUserPermissionInDB,
                actualPermissions: allTraineeUserPermissions
            );
        }
        private void CheckAdminRoles(Role adminRole)
        {
            var adminPermissionsInDB = _context
                .Permissions
                .IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var allPermissions = PermissionFinder
                .GetAllPermissions(new WazzifniAuthorizationProvider())
                .Select(p => p.Name)
                .ToList();

            GrantPermissionToRole(
                role: adminRole,
                alreadyIncludedPermissions: adminPermissionsInDB,
                actualPermissions: allPermissions
            );
        }
        private void GrantPermissionToRole(Role role, List<string> alreadyIncludedPermissions, List<string> actualPermissions)
        {
            var permissionsNotIncluded = PermissionFinder
                .GetAllPermissions(new WazzifniAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) && !alreadyIncludedPermissions.Contains(p.Name) && actualPermissions.Contains(p.Name))
                .ToList();

            if (permissionsNotIncluded.Any())
            {
                _context.Permissions.AddRange(
                    permissionsNotIncluded.Select(permission => new RolePermissionSetting
                    {
                        RoleId = role.Id,
                        IsGranted = true,
                        TenantId = _tenantId,
                        Name = permission.Name,
                    })
                );

                _context.SaveChanges();
            }
        }
    }
}
