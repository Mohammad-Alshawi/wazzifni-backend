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

        [AbpAllowAnonymous]
        public MobileLinksDto GetMobileLinks()
        {
            var MobileLinkDto = new MobileLinksDto()
            {
                AndroidLinkForBasic = SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBasic),
                IosLinkForBasic = SettingManager.GetSettingValue(AppSettingNames.IosLinkForBasic),
                AndroidLinkForBusiness = SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBusiness),
                IosLinkForBusiness = SettingManager.GetSettingValue(AppSettingNames.IosLinkForBusiness),
            };
            return MobileLinkDto;
        }

        public void SetMobileLinks(MobileLinksDto input)
        {

            if (input.IosLinkForBasic != SettingManager.GetSettingValue(AppSettingNames.IosLinkForBasic))
                SettingManager.ChangeSettingForApplication(AppSettingNames.IosLinkForBasic,
                    input.IosLinkForBasic.ToString());

            if (input.AndroidLinkForBasic != SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBasic))
                SettingManager.ChangeSettingForApplication(AppSettingNames.AndroidLinkForBasic,
                    input.AndroidLinkForBasic.ToString());

            if (input.IosLinkForBusiness != SettingManager.GetSettingValue(AppSettingNames.IosLinkForBusiness))
                SettingManager.ChangeSettingForApplication(AppSettingNames.IosLinkForBusiness,
                    input.IosLinkForBusiness.ToString());

            if (input.AndroidLinkForBusiness != SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBusiness))
                SettingManager.ChangeSettingForApplication(AppSettingNames.AndroidLinkForBusiness,
                    input.AndroidLinkForBusiness.ToString());


        }
    }
}
