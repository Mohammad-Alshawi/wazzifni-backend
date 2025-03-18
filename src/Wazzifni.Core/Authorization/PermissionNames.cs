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

        public const string Companies = "Companies";
        public const string Companies_Read = Companies + "." + "Read";
        public const string Companies_Create = Companies + "." + "Create";
        public const string Companies_Delete = Companies + "." + "Delete";
        public const string Companies_Update = Companies + "." + "Update";

        public const string WorkPosts = "WorkPosts";
        public const string WorkPosts_Read = WorkPosts + "." + "Read";
        public const string WorkPosts_Create = WorkPosts + "." + "Create";
        public const string WorkPosts_Delete = WorkPosts + "." + "Delete";
        public const string WorkPosts_Update = WorkPosts + "." + "Update";


        public const string WorkApplications = "WorkApplications";
        public const string WorkApplications_Create = WorkApplications + "." + "Create";
        public const string WorkApplications_Delete = WorkApplications + "." + "Delete";
        public const string WorkApplications_Approve = WorkApplications + "." + "Approve";
        public const string WorkApplications_Reject = WorkApplications + "." + "Reject";
        public const string WorkApplications_Update = WorkApplications + "." + "Update";

        public const string Advertisements_CUD = "Advertisements_CUD";


    }
}
