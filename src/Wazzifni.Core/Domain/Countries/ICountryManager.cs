using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wazzifni.Countries;

namespace Wazzifni.Domain.Countries
{
    public interface ICountryManager : IDomainService
    {

        /// <summary>
        /// function to check if country is already exist
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<bool> CheckIfCountryExist(List<CountryTranslation> Translations);
        /// <summary>
        /// GetEntityByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Country> GetEntityByIdAsync(int id);

        Task<Country> GetLiteEntityByIdAsync(int id);
        Task<List<string>> GetAllCountryNameForAutoComplete(string keyword);


    }
}
