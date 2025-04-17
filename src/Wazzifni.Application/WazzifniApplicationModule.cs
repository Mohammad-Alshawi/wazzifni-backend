using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Wazzifni.Authorization;
using Wazzifni.Cities.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Countries;
using Wazzifni.Countries.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.Courses.Dto;
using Wazzifni.CourseTags.Dto;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.CourseTags;
using Wazzifni.Domain.Regions;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.Universities;
using Wazzifni.NotificationService;
using Wazzifni.Skills.Dto;
using Wazzifni.Universities.Dto;

namespace Wazzifni
{
    [DependsOn(
        typeof(WazzifniCoreModule),
        typeof(AbpAutoMapperModule))]
    public class WazzifniApplicationModule : AbpModule
    {
        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();

            Configuration.Notifications.Notifiers.Add<FirebaseRealTimeNotifier>();

        }
        public override void PreInitialize()
        {
            FirebaseApp.Create(new AppOptions() { Credential = GoogleCredential.FromFile(@"Firebase/firebasesettings.json") });

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
                configuration.CreateMultiLingualMap<Company, CompanyTranslation, LiteCompanyDto>(context).TranslationMap
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About));
                configuration.CreateMultiLingualMap<Company, CompanyTranslation, CompanyDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About));
                #endregion

                #region Skill
                // Skill Translation Configuration
                configuration.CreateMultiLingualMap<Skill, SkillTranslation, LiteSkillDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<Skill, SkillTranslation, SkillDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<Skill, SkillTranslation, SkillDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

                #endregion


                #region University
                // University Translation Configuration
                configuration.CreateMultiLingualMap<University, UniversityTranslation, LiteUniversityDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<University, UniversityTranslation, UniversityDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<University, UniversityTranslation, UniversityDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

                #endregion

                #region CourseCategory
                // CourseCategory Translation Configuration
                configuration.CreateMultiLingualMap<CourseCategory, CourseCategoryTranslation, LiteCourseCategoryDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<CourseCategory, CourseCategoryTranslation, CourseCategoryDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<CourseCategory, CourseCategoryTranslation, CourseCategoryDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<CourseCategory, CourseCategoryTranslation, LiteCourseCategory>(context).TranslationMap
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                #endregion

                #region CourseTag
                // CourseTag Translation Configuration
                configuration.CreateMultiLingualMap<CourseTag, CourseTagTranslation, LiteCourseTagDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<CourseTag, CourseTagTranslation, CourseTagDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<CourseTag, CourseTagTranslation, CourseTagDto>(context).TranslationMap
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                configuration.CreateMultiLingualMap<CourseTag, CourseTagTranslation, LiteCourseTagDto>(context).TranslationMap
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                #endregion



                #region Course
                // Course Translation Configuration
                configuration.CreateMultiLingualMap<Course, CourseTranslation, CourseLiteDto>(context).TranslationMap
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
                configuration.CreateMultiLingualMap<Course, CourseTranslation, CourseDetailsDto>(context).TranslationMap
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

                #endregion
            }
        }
    }
}