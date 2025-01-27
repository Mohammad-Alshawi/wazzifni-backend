using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Wazzifni.Controllers
{
    public abstract class WazzifniControllerBase: AbpController
    {
        protected WazzifniControllerBase()
        {
            LocalizationSourceName = WazzifniConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
