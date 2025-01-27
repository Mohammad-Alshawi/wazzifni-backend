using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AutoMapper;
using Wazzifni.Authorization;
using Wazzifni.Cities.Dto;
using Wazzifni.Countries;
using Wazzifni.Countries.Dto;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Companies.Dto;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.Regions;

namespace Wazzifni
{
    [DependsOn(
        typeof(WazzifniCoreModule),
        typeof(AbpAutoMapperModule))]
    public class WazzifniApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WazzifniAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WazzifniApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                CustomDtoMapper.CreateMappings(configuration, new MultiLingualMapContext(
                    IocManager.Resolve<ISettingManager>()
                ));
            });
        }


        internal static class CustomDtoMapper
        {
            public static void CreateMappings(IMapperConfigurationExpression configuration, MultiLingualMapContext context)
            {





                #region Country
                // Country Translation Configuration
                configuration.CreateMultiLingualMap<Country, CountryTranslation, CountryDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<Country, CountryTranslation, CountryDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                #endregion

                #region City
                // City Translation Configuration
                configuration.CreateMultiLingualMap<City, CityTranslation, LiteCityDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<City, CityTranslation, CityDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<City, CityTranslation, CityDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<City, CityTranslation, LiteCity>(context).TranslationMap
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                #endregion

                #region Region
                // Region Translation Configuration

                configuration.CreateMultiLingualMap<Region, RegionTranslation, LiteRegionCityDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

                #endregion
                #region Company
                // Company Translation Configuration
                configuration.CreateMultiLingualMap<Company, CompanyTranslation, CompanyDetailsDto>(context).TranslationMap
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About));
                configuration.CreateMultiLingualMap<Company, CompanyTranslation, CompanyDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About));
                #endregion
            }
        }
    }
}