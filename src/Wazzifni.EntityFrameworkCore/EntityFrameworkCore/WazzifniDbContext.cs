using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;
using Wazzifni.Awards;
using Wazzifni.Countries;
using Wazzifni.Domain.Advertisiments;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.ChangedPhoneNumber;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.CourseComments;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.CourseTags;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.Feedbacks;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.Regions;
using Wazzifni.Domain.RegisterdPhoneNumbers;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.Teachers;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domain.Universities;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Domain.WorkPostFaveorites;
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

        public virtual DbSet<FavoriteWorkPost> FavoriteWorkPosts { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Advertisiment> Advertisiments { get; set; }

        public virtual DbSet<Trainee>  Trainees { get; set; }
        public virtual DbSet<University>  Universities { get; set; }
        public virtual DbSet<UniversityTranslation>  UniversityTranslations { get; set; }
        public virtual DbSet<CourseCategory>  CourseCategories { get; set; }
        public virtual DbSet<CourseCategoryTranslation>  CourseCategoryTranslations { get; set; }
        public virtual DbSet<CourseTag> CourseTags { get; set; }
        public virtual DbSet<CourseTagTranslation> CourseTagTranslations { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseTranslation> CourseTranslations { get; set; }

        public virtual DbSet<CourseComment>  CourseComments { get; set; }
        public WazzifniDbContext(DbContextOptions<WazzifniDbContext> options)
            : base(options)
        {
        }
    }
}
