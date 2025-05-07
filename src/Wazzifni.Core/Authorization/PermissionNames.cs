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
        public const string Companies_Approve = Companies + "." + "Approve";
        public const string Companies_Reject = Companies + "." + "Reject";


        public const string WorkPosts = "WorkPosts";
        public const string WorkPosts_Read = WorkPosts + "." + "Read";
        public const string WorkPosts_Create = WorkPosts + "." + "Create";
        public const string WorkPosts_Delete = WorkPosts + "." + "Delete";
        public const string WorkPosts_Update = WorkPosts + "." + "Update";
        public const string WorkPosts_Approve = WorkPosts + "." + "Approve";
        public const string WorkPosts_Reject = WorkPosts + "." + "Reject";

        public const string WorkPosts_SwitchFeatured = WorkPosts + "." + "SwitchFeatured";



        public const string WorkApplications = "WorkApplications";
        public const string WorkApplications_Read = WorkApplications + "." + "Read";
        public const string WorkApplications_Create = WorkApplications + "." + "Create";
        public const string WorkApplications_Delete = WorkApplications + "." + "Delete";
        public const string WorkApplications_Approve = WorkApplications + "." + "Approve";
        public const string WorkApplications_Reject = WorkApplications + "." + "Reject";
        public const string WorkApplications_Update = WorkApplications + "." + "Update";
        public const string WorkApplications_AcceptToSendToCompany = WorkApplications + "." + "AcceptToSendToCompany";


        public const string Advertisements_CUD = "Advertisements_CUD";

        public const string CourseRegistrationRequests = "CourseRegistrationRequests";
        public const string CourseRegistrationRequests_Create = CourseRegistrationRequests + "." + "Create";
        public const string CourseRegistrationRequests_Delete = CourseRegistrationRequests + "." + "Delete";
        public const string CourseRegistrationRequests_Approve = CourseRegistrationRequests + "." + "Approve";
        public const string CourseRegistrationRequests_Reject = CourseRegistrationRequests + "." + "Reject";
        public const string CourseRegistrationRequests_Update = CourseRegistrationRequests + "." + "Update";

        public const string Courses = "Courses";
        public const string Courses_Rate = Courses + "." + "Rate";
        public const string Courses_Read = Courses + "." + "Read";


        public const string AppManagement = "AppManagement";
        public const string AppManagement_Read = AppManagement + "." + "Read";


        public const string CourseTag = "CourseTag";
        public const string CourseTag_Read = CourseTag + "." + "Read";
        public const string CourseTag_Create = CourseTag + "." + "Create";
        public const string CourseTag_Delete = CourseTag + "." + "Delete";
        public const string CourseTag_Update = CourseTag + "." + "Update";

        public const string CourseCategory = "CourseCategory";
        public const string CourseCategory_Read = CourseCategory + "." + "Read";
        public const string CourseCategory_Create = CourseCategory + "." + "Create";
        public const string CourseCategory_Delete = CourseCategory + "." + "Delete";
        public const string CourseCategory_Update = CourseCategory + "." + "Update";

        public const string Teacher = "Teacher";
        public const string Teacher_Read = Teacher + "." + "Read";
        public const string Teacher_Create = Teacher + "." + "Create";
        public const string Teacher_Delete = Teacher + "." + "Delete";
        public const string Teacher_Update = Teacher + "." + "Update";


        public const string CourseComment = "CourseComment";
        public const string CourseComment_Read = CourseComment + "." + "Read";
        public const string CourseComment_Create = CourseComment + "." + "Create";
        public const string CourseComment_Delete = CourseComment + "." + "Delete";
        public const string CourseComment_Update = CourseComment + "." + "Update";

        public const string Skill = "Skill";
        public const string Skill_Create = Skill + "." + "Create";
        public const string Skill_Delete = Skill + "." + "Delete";
        public const string Skill_Update = Skill + "." + "Update";

        public const string SpokenLanguage = "SpokenLanguage";
        public const string SpokenLanguage_Create = SpokenLanguage + "." + "Create";
        public const string SpokenLanguage_Delete = SpokenLanguage + "." + "Delete";
        public const string SpokenLanguage_Update = SpokenLanguage + "." + "Update";

        public const string University = "University";
        public const string University_Create = University + "." + "Create";
        public const string University_Delete = University + "." + "Delete";
        public const string University_Update = University + "." + "Update";
    }
}
