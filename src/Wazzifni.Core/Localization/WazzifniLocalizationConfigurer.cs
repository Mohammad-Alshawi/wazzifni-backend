using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Wazzifni.Localization
{
    public static class WazzifniLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(WazzifniConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(WazzifniLocalizationConfigurer).GetAssembly(),
                        "Wazzifni.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
