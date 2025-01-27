using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Localization.SourceFiles;

namespace Wazzifni.Domain.Regions
{
    public class RegionManager : DomainService, IRegionManager
    {
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<RegionTranslation> _regionTranslationRepository;

        public RegionManager(IRepository<Region> regionRepository,
            IRepository<RegionTranslation> regionTranslationRepository)
        {
            _regionRepository = regionRepository;
            _regionTranslationRepository = regionTranslationRepository;
        }
        /// If the ID is not equal to -1, the method fetches the Region entity
        /// with the specified ID
        /// Value -1 to handle input errors from the frontend and return a default region
        public async Task<Region> GetEntityByIdAsync(int id)
        {
            var entity = new Region();

            if (id != -1)
            {
                entity = await _regionRepository.GetAll()
                    .Include(c => c.Translations)
                    .Include(c => c.City).ThenInclude(c => c.Translations)
                    .Include(c => c.City).ThenInclude(c => c.Country)
                    .ThenInclude(c => c.Translations)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    throw new EntityNotFoundException(typeof(Region), id);
            }
            else
            {
                entity = await _regionRepository.GetAll()
                        .Include(c => c.Translations)
                        .Include(c => c.City).ThenInclude(c => c.Translations)
                        .Include(c => c.City).ThenInclude(c => c.Country)
                        .ThenInclude(c => c.Translations)
                        .FirstOrDefaultAsync(x => x.IsActive);

            }
            return entity;
        }

        public async Task<Region> GetLiteEntityByIdAsync(int id)
        {
            var entity = await _regionRepository.GetAsync(id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(Region), id);
            return entity;
        }
        public async Task IsEntityExistAsync(int id)
        {
            var entity = await _regionRepository.GetAsync(id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(Region), id);
        }

        public async Task<bool> CheckIfRegionIsExist(List<RegionTranslation> Translations)
        {
            var regions = await _regionTranslationRepository.GetAll().ToListAsync();
            foreach (var Translation in Translations)
            {
                foreach (var region in regions)
                    if (region.Name == Translation.Name && region.Language == Translation.Language)
                        return true;
            }
            return false;
        }

        public async Task<List<string>> GetAllRegionNameForAutoComplete(string inputAutoComplete)
        {
            return await _regionTranslationRepository.GetAll().Where(x => x.Name.Contains(inputAutoComplete)).Select(x => x.Name).ToListAsync();
        }

        public async Task<List<Region>> CheckAndGetRegionsById(List<int> regionsIds)
        {
            List<Region> regions = new List<Region>();
            foreach (var Id in regionsIds)
            {
                if (!await CheckIfRegionIsExist(Id))
                    throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Region + Id.ToString()));
                var region = await GetLiteEntityByIdAsync(Id);
                regions.Add(region);
            }
            return regions;
        }
        public async Task<bool> CheckIfRegionIsExist(int cityId)
        {
            return await _regionRepository.GetAll().AnyAsync(x => x.Id == cityId && x.IsActive);
        }
    }
}

