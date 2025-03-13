using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.IndividualUserProfiles
{
    public interface IProfileManager : IDomainService
    {
        Task<long> InitateProfileForBasicUser(long UserId, int CityId);

        Task<Profile> GetEntityByIdAsync(long ProfileId);

        Task<Profile> GetEntityByUserIdAsync(long UserId);

        Task<long> GetProfileIdByUserId(long userId);

        Task DeleteProfileByUserId(long userId);

    }
}
