namespace Wazzifni.Authorization
{
    public static class PermissionNames
    {
        public const string Pages_Tenants = "Pages.Tenants";

        public const string Pages_Users = "Pages.Users";
        public const string Pages_Users_Activation = "Pages.Users.Activation";

        public const string Pages_Roles = "Pages.Roles";
        public const string Roles_List = Pages_Roles + ".List";
        public const string Roles_Create = Pages_Roles + ".Create";
        public const string Roles_Update = Pages_Roles + ".Update";
        public const string Roles_Delete = Pages_Roles + ".Delete";
        public const string Roles_Get = Pages_Roles + ".Get";
        public const string Roles_GetAllPermission = Roles_List + ".Permission";


        public const string Users_List = Pages_Users + ".List";
        public const string Users_Create = Pages_Users + ".Create";
        public const string Users_Update = Pages_Users + ".Update";
        public const string Users_Delete = Pages_Users + ".Delete";

        public const string Accounts = "Accounts";
        public const string Accounts_Read = Accounts + "." + "Read";
        public const string Accounts_Delete = Accounts + "." + "Delete";
        public const string Accounts_Update = Accounts + "." + "Update";
    }
}
