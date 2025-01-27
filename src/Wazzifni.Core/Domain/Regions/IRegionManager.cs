using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Regions
{
    public interface IRegionManager : IDomainService
    {
        Task<Region> GetEntityByIdAsync(int id);
        Task<bool> CheckIfRegionIsExist(List<RegionTranslation> Translations);

        Task<Region> GetLiteEntityByIdAsync(int id);
        Task IsEntityExistAsync(int id);
        Task<List<string>> GetAllRegionNameForAutoComplete(string inputAutoComplete);
        Task<List<Region>> CheckAndGetRegionsById(List<int> regionsIds);
    }
}
