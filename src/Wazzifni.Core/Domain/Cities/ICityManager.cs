using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Cities
{
    // ICityManager
    public interface ICityManager : IDomainService
    {


        /// check if city name is already exist in same country
        /// </summary>
        /// <param name="CityName"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<bool> CheckIfCityIsExist(List<CityTranslation> Translations);

        /// <summary>
        /// GetCitiesCounts
        /// </summary>
        /// <returns></returns>
        Task<int> GetCitiesCounts();

        /// <summary>
        /// GetEntityByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<City> GetEntityByIdAsync(int id);

        Task<City> GetLiteEntityByIdAsync(int id);

        Task<List<string>> GetAllCityNameForAutoComplete(string inputAutoComplete);
        Task<City> GetCityByIdAsync(int id);
    }
}
