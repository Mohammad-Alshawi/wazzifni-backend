using Abp.Domain.Repositories;
using Abp.Domain.Services;
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


        public async Task<long> InitateProfileForBasicUser(long UserId, int CityId)
        {
            var profile = new Profile { UserId = UserId, CityId = CityId };

            return await repository.InsertAndGetIdAsync(profile);

        }
    }
}
