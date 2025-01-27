using Abp.Authorization;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;

namespace Wazzifni.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
