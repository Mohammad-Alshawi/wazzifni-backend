using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Wazzifni.Configuration;

namespace Wazzifni.Web.Host.Startup
{
    [DependsOn(
       typeof(WazzifniWebCoreModule))]
    public class WazzifniWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WazzifniWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WazzifniWebHostModule).GetAssembly());
        }
    }
}
