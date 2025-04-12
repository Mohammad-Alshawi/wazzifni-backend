using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wazzifni.Domain.CourseCategories
{
    // ICourseCategoryManager
    public interface ICourseCategoryManager : IDomainService
    {


        /// check if CourseCategory name is already exist in same country
        /// </summary>
        /// <param name="CourseCategoryName"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<bool> CheckIfCourseCategoryIsExist(List<CourseCategoryTranslation> Translations);

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
        Task<CourseCategory> GetEntityByIdAsync(int id);

        Task<CourseCategory> GetLiteEntityByIdAsync(int id);

        Task<List<string>> GetAllCourseCategoryNameForAutoComplete(string inputAutoComplete);
        Task<CourseCategory> GetCourseCategoryByIdAsync(int id);
    }
}
