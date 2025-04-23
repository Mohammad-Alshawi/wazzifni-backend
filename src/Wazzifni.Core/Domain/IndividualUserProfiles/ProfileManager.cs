using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;

namespace Wazzifni.Domain.IndividualUserProfiles
{
    public class ProfileManager : DomainService, IProfileManager
    {
        private readonly IRepository<Profile, long> repository;
        private readonly UserManager userManager;

        public ProfileManager(IRepository<Profile, long> repository, UserManager userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public async Task<Profile> GetEntityByIdAsync(long ProfileId)
        {
            return await repository
                .GetAllIncluding(x => x.User)
                .Include(x => x.City)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Skills)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Educations)
                .Include(x => x.WorkExperiences)
                .Include(x => x.Awards)
                .Include(x => x.SpokenLanguages).ThenInclude(x => x.SpokenLanguage)
                .AsNoTracking().Where(x => x.Id == ProfileId).FirstOrDefaultAsync();
        }


        public async Task<Profile> GetEntityByIdASTrackingAsync(long ProfileId)
        {
            return await repository
                .GetAllIncluding(x => x.User)
                .Include(x => x.City)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Skills)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Educations)
                .Include(x => x.WorkExperiences)
                .Include(x => x.Awards)
                .Include(x => x.SpokenLanguages).ThenInclude(x => x.SpokenLanguage)
                .Where(x => x.Id == ProfileId).FirstOrDefaultAsync();
        }

        public async Task<Profile> GetEntityByUserIdAsync(long UserId)
        {
            return await repository
              .GetAllIncluding(x => x.User)
              .Include(x => x.City)
              .ThenInclude(x => x.Translations)
              .Include(x => x.Skills)
              .ThenInclude(x => x.Translations)
              .Include(x => x.Educations)
              .Include(x => x.WorkExperiences)
              .Include(x => x.Awards)
              .Include(x => x.SpokenLanguages).ThenInclude(x => x.SpokenLanguage)
              .Where(x => x.UserId == UserId).FirstOrDefaultAsync();
        }

        public async Task<long> InitateProfileForBasicUser(long UserId, int CityId)
        {
            var profile = new Profile { UserId = UserId, CityId = CityId };

            return await repository.InsertAndGetIdAsync(profile);

        }

        public async Task<long> GetProfileIdByUserId(long userId)
        {
            return await repository.GetAll().Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task DeleteProfileByUserId(long userId)
        {
            var profile = await repository.GetAll()
                           .Include(x => x.Awards)
                           .Include(x => x.Educations)
                           .Include(x => x.SpokenLanguages)
                           .Include(x => x.Skills)
                           .Include(x => x.WorkExperiences)
                           .Include(x => x.Applications)
                           .Where(x => x.UserId == userId).FirstOrDefaultAsync();

            profile.Awards.Clear();
            profile.Educations.Clear();
            profile.SpokenLanguages.Clear();
            profile.Skills.Clear();
            profile.WorkExperiences.Clear();
            profile.Applications.Clear();

            await repository.DeleteAsync(profile);
        }

        public async Task<Profile> GetEntityByIdWithUserAsync(long ProfileId)
        {
            return await repository
                .GetAllIncluding(x => x.User).Where(x => x.Id == ProfileId).FirstOrDefaultAsync();
        }
    }
}
