using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Wazzifni.Authorization
{
    public class WazzifniAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Users_List, L("UsersList"));
            context.CreatePermission(PermissionNames.Users_Create, L("UserCreate"));
            context.CreatePermission(PermissionNames.Users_Update, L("UserUpdate"));
            context.CreatePermission(PermissionNames.Users_Delete, L("UserDelete"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));

            context.CreatePermission(PermissionNames.Accounts, L("Accounts"));
            context.CreatePermission(PermissionNames.Accounts_Read, L("AccountsRead"));
            context.CreatePermission(PermissionNames.Accounts_Update, L("AccountsUpdate"));
            context.CreatePermission(PermissionNames.Accounts_Delete, L("AccountsDelete"));

            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Roles_Create, L("RoleCreate"));
            context.CreatePermission(PermissionNames.Roles_Update, L("RoleUpdate"));
            context.CreatePermission(PermissionNames.Roles_Delete, L("RoleDelete"));
            context.CreatePermission(PermissionNames.Roles_List, L("RolesList"));
            context.CreatePermission(PermissionNames.Roles_GetAllPermission, L("RolesPermission"));
            context.CreatePermission(PermissionNames.Roles_Get, L("RoleGet"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, WazzifniConsts.LocalizationSourceName);
        }
    }
}
