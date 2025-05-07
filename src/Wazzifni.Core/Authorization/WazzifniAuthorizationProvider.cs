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


            context.CreatePermission(PermissionNames.Companies, L("Companies"));
            context.CreatePermission(PermissionNames.Companies_Read, L("CompaniesRead"));
            context.CreatePermission(PermissionNames.Companies_Create, L("CompaniesCreate"));
            context.CreatePermission(PermissionNames.Companies_Update, L("CompaniesUpdate"));
            context.CreatePermission(PermissionNames.Companies_Delete, L("CompaniesDelete"));
            context.CreatePermission(PermissionNames.Companies_Approve, L("CompaniesApprove"));
            context.CreatePermission(PermissionNames.Companies_Reject, L("CompaniesReject"));

            context.CreatePermission(PermissionNames.WorkPosts, L("WorkPosts"));
            context.CreatePermission(PermissionNames.WorkPosts_Read, L("WorkPostsRead"));
            context.CreatePermission(PermissionNames.WorkPosts_Create, L("WorkPostsCreate"));
            context.CreatePermission(PermissionNames.WorkPosts_Update, L("WorkPostsUpdate"));
            context.CreatePermission(PermissionNames.WorkPosts_Delete, L("WorkPostsDelete"));
            context.CreatePermission(PermissionNames.WorkPosts_Approve, L("WorkPostsApprove"));
            context.CreatePermission(PermissionNames.WorkPosts_Reject, L("WorkPostsReject"));
            context.CreatePermission(PermissionNames.WorkPosts_SwitchFeatured, L("WorkPostsSwitchFeatured"));




            context.CreatePermission(PermissionNames.WorkApplications, L("WorkApplications"));
            context.CreatePermission(PermissionNames.WorkApplications_Read, L("WorkApplicationsRead"));
            context.CreatePermission(PermissionNames.WorkApplications_Approve, L("WorkApplicationsApprove"));
            context.CreatePermission(PermissionNames.WorkApplications_Create, L("WorkApplicationsCreate"));
            context.CreatePermission(PermissionNames.WorkApplications_Reject, L("WorkApplicationsReject"));
            context.CreatePermission(PermissionNames.WorkApplications_Delete, L("WorkApplicationsDelete"));
            context.CreatePermission(PermissionNames.WorkApplications_Update, L("WorkApplicationsUpdate"));
            context.CreatePermission(PermissionNames.WorkApplications_AcceptToSendToCompany, L("WorkApplicationsAcceptToSendToCompany"));


            context.CreatePermission(PermissionNames.Advertisements_CUD, L("Advertisements_CUD"));

            context.CreatePermission(PermissionNames.CourseRegistrationRequests, L("CourseRegistrationRequests"));
            context.CreatePermission(PermissionNames.CourseRegistrationRequests_Approve, L("CourseRegistrationRequestsApprove"));
            context.CreatePermission(PermissionNames.CourseRegistrationRequests_Create, L("CourseRegistrationRequestsCreate"));
            context.CreatePermission(PermissionNames.CourseRegistrationRequests_Reject, L("CourseRegistrationRequestsReject"));
            context.CreatePermission(PermissionNames.CourseRegistrationRequests_Delete, L("CourseRegistrationRequestsDelete"));
            context.CreatePermission(PermissionNames.CourseRegistrationRequests_Update, L("CourseRegistrationRequestsUpdate"));

            context.CreatePermission(PermissionNames.Courses, L("Courses"));
            context.CreatePermission(PermissionNames.Courses_Rate, L("CoursesRate"));
            context.CreatePermission(PermissionNames.Courses_Read, L("CoursesRead"));

            context.CreatePermission(PermissionNames.AppManagement, L("AppManagement"));
            context.CreatePermission(PermissionNames.AppManagement_Read, L("AppManagementRead"));




            context.CreatePermission(PermissionNames.CourseTag, L("CourseTag"));
            context.CreatePermission(PermissionNames.CourseTag_Read, L("CourseTagRead"));
            context.CreatePermission(PermissionNames.CourseTag_Create, L("CourseTagCreate"));
            context.CreatePermission(PermissionNames.CourseTag_Update, L("CourseTagUpdate"));
            context.CreatePermission(PermissionNames.CourseTag_Delete, L("CourseTagDelete"));

            context.CreatePermission(PermissionNames.CourseCategory, L("CourseCategory"));
            context.CreatePermission(PermissionNames.CourseCategory_Read, L("CourseCategoryRead"));
            context.CreatePermission(PermissionNames.CourseCategory_Create, L("CourseCategoryCreate"));
            context.CreatePermission(PermissionNames.CourseCategory_Update, L("CourseCategoryUpdate"));
            context.CreatePermission(PermissionNames.CourseCategory_Delete, L("CourseCategoryDelete"));

            context.CreatePermission(PermissionNames.Teacher, L("Teacher"));
            context.CreatePermission(PermissionNames.Teacher_Read, L("TeacherRead"));
            context.CreatePermission(PermissionNames.Teacher_Create, L("TeacherCreate"));
            context.CreatePermission(PermissionNames.Teacher_Update, L("TeacherUpdate"));
            context.CreatePermission(PermissionNames.Teacher_Delete, L("TeacherDelete"));

            context.CreatePermission(PermissionNames.Skill, L("Skill"));
            context.CreatePermission(PermissionNames.Skill_Create, L("SkillCreate"));
            context.CreatePermission(PermissionNames.Skill_Update, L("SkillUpdate"));
            context.CreatePermission(PermissionNames.Skill_Delete, L("SkillDelete"));

            context.CreatePermission(PermissionNames.CourseComment, L("CourseComment"));
            context.CreatePermission(PermissionNames.CourseComment_Read, L("CourseCommentRead"));
            context.CreatePermission(PermissionNames.CourseComment_Create, L("CourseCommentCreate"));
            context.CreatePermission(PermissionNames.CourseComment_Update, L("CourseCommentUpdate"));
            context.CreatePermission(PermissionNames.CourseComment_Delete, L("CourseCommentDelete"));

            context.CreatePermission(PermissionNames.SpokenLanguage, L("SpokenLanguage"));
            context.CreatePermission(PermissionNames.SpokenLanguage_Create, L("SpokenLanguageCreate"));
            context.CreatePermission(PermissionNames.SpokenLanguage_Update, L("SpokenLanguageUpdate"));
            context.CreatePermission(PermissionNames.SpokenLanguage_Delete, L("SpokenLanguageDelete"));

            context.CreatePermission(PermissionNames.University, L("University"));
            context.CreatePermission(PermissionNames.University_Create, L("UniversityCreate"));
            context.CreatePermission(PermissionNames.University_Update, L("UniversityUpdate"));
            context.CreatePermission(PermissionNames.University_Delete, L("UniversityDelete"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, WazzifniConsts.LocalizationSourceName);
        }
    }
}
