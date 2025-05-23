﻿using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;
using Wazzifni.Configuration;
using Wazzifni.Localization;
using Wazzifni.MultiTenancy;
using Wazzifni.Timing;

namespace Wazzifni
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class WazzifniCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            WazzifniLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = WazzifniConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            Configuration.Localization.Languages.Add(new LanguageInfo("ku", "Kurdî", "famfamfam-flag-icon-ku"));

            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = WazzifniConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = WazzifniConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WazzifniCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
