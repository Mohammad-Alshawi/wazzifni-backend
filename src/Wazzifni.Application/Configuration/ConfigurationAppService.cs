using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Wazzifni.Configuration.Dto;

namespace Wazzifni.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : WazzifniAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
