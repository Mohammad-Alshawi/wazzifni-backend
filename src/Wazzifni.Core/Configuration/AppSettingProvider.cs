using System.Collections.Generic;
using Abp.Configuration;

namespace Wazzifni.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()),
                new SettingDefinition(AppSettingNames.FileSize, "25", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()),
                new SettingDefinition(AppSettingNames.ImageSize, "100", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()),


                new SettingDefinition(AppSettingNames.IosLinkForBasic, "https://www.Wazzifni.net/download?", scopes: SettingScopes.Application | SettingScopes.All),
                new SettingDefinition(AppSettingNames.AndroidLinkForBasic, "https://www.Wazzifni.net/download?", scopes: SettingScopes.Application | SettingScopes.All),
                new SettingDefinition(AppSettingNames.IosLinkForBusiness, "https://www.Wazzifni.net/download?", scopes: SettingScopes.Application | SettingScopes.All),
                new SettingDefinition(AppSettingNames.AndroidLinkForBusiness, "https://www.Wazzifni.net/download?", scopes: SettingScopes.Application | SettingScopes.All),
            };
        }
    }
}
