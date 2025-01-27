using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Wazzifni.EntityFrameworkCore;
using Wazzifni.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Wazzifni.Web.Tests
{
    [DependsOn(
        typeof(WazzifniWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class WazzifniWebTestModule : AbpModule
    {
        public WazzifniWebTestModule(WazzifniEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WazzifniWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(WazzifniWebMvcModule).Assembly);
        }
    }
}