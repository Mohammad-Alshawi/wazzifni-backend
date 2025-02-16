using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;
using Wazzifni.Awards;
using Wazzifni.Countries;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.ChangedPhoneNumber;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.Regions;
using Wazzifni.Domain.RegisterdPhoneNumbers;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.Domains.UserVerficationCodes;
using Wazzifni.MultiTenancy;

namespace Wazzifni.EntityFrameworkCore
{
    public class WazzifniDbContext : AbpZeroDbContext<Tenant, Role, User, WazzifniDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<UserVerficationCode> UserVerficationCodes { get; set; }
        public virtual DbSet<RegisterdPhoneNumber> RegisterdPhoneNumbers { get; set; }
        public virtual DbSet<ChangedPhoneNumberForUser> ChangedPhoneNumberForUsers { get; set; }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryTranslation> CountryTranslations { get; set; }

        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<RegionTranslation> RegionTranslations { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<CityTranslation> CityTranslations { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyTranslation> CompanyTranslations { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<SkillTranslation> SkillTranslations { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<WorkExperience> WorkExperiences { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Award> Awards { get; set; }

        public virtual DbSet<SpokenLanguage> SpokenLanguages { get; set; }

        public virtual DbSet<SpokenLanguageValue> SpokenLanguageValue { get; set; }
        public virtual DbSet<WorkPost> WorkPosts { get; set; }

        public virtual DbSet<WorkApplication> WorkApplications { get; set; }

        public WazzifniDbContext(DbContextOptions<WazzifniDbContext> options)
            : base(options)
        {
        }
    }
}
