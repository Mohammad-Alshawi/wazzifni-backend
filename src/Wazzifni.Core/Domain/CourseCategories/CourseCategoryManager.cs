using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Countries;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.Regions;

namespace Wazzifni.Domain.CourseCategories
{
    //CourseCategory manager
    public class CourseCategoryManager : DomainService, ICourseCategoryManager
    {
        private readonly IRepository<CourseCategory> _CourseCategoryRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly ICountryManager _countryManager;
        private readonly IRegionManager _regionManager;
        private readonly IRepository<CourseCategoryTranslation> _CourseCategoryTranslationRepository;


        public CourseCategoryManager(IRepository<CourseCategory> CourseCategoryRepository,
            IRepository<Country> countryRepository,
            ICountryManager countryManager,
            IRegionManager regionManager,
            IRepository<CourseCategoryTranslation> CourseCategoryTranslationRepository)
        {
            _CourseCategoryRepository = CourseCategoryRepository;
            _countryRepository = countryRepository;
            _countryManager = countryManager;
            _regionManager = regionManager;
            _CourseCategoryTranslationRepository = CourseCategoryTranslationRepository;
        }

        public async Task<bool> CheckIfCourseCategoryIsExist(List<CourseCategoryTranslation> Translations)
        {

            var cities = await _CourseCategoryTranslationRepository.GetAll().ToListAsync();
            foreach (var Translation in Translations)
            {
                foreach (var CourseCategory in cities)
                    if (CourseCategory.Name == Translation.Name && CourseCategory.Language == Translation.Language)
                        return true;
            }

            return false;
        }

        public async Task<int> GetCitiesCounts()
        {
            return await _CourseCategoryRepository.GetAll().Where(x => x.IsActive).CountAsync();
        }

        public async Task<CourseCategory> GetEntityByIdAsync(int id)
        {
            var entity = await _CourseCategoryRepository.GetAll()
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(CourseCategory), id);
            return entity;
        }

        public async Task<CourseCategory> GetCourseCategoryByIdAsync(int id)
        {
            var entity = await _CourseCategoryRepository.GetAll()
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(CourseCategory), id);
            return entity;
        }


        public async Task<CourseCategory> GetLiteEntityByIdAsync(int id)
        {
            var entity = await _CourseCategoryRepository.GetAsync(id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(CourseCategory), id);
            return entity;
        }

        public async Task<List<string>> GetAllCourseCategoryNameForAutoComplete(string inputAutoComplete)
        {
            return await _CourseCategoryTranslationRepository.GetAll().Where(x => x.Name.Contains(inputAutoComplete)).Select(x => x.Name).ToListAsync();
        }

    }
}
