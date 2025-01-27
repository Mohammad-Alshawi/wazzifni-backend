using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using KeyFinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Cities.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Countries;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Cities
{

    public class CityAppService :
        WazzifniAsyncCrudAppService<City, CityDetailsDto, int, LiteCityDto, PagedCityResultRequestDto, CreateCityDto, UpdateCityDto>,
        ICityAppService
    {

        public const string CacheName_GetCities = "GET-CITIES";

        private readonly UserManager _userManager;
        private readonly CityManager _cityManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<City> _cityRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IRepository<CityTranslation> _cityTranslationRepository;


        public CityAppService(
            CityManager cityManager,
            UserManager userManager,
            ICacheManager cacheManager,
            IRepository<City> repository,
            CountryManager countryManager,
            IRepository<City> cityRepository,
            IAttachmentManager attachmentManager,
            IRepository<CityTranslation> cityTranslationRepository
        ) : base(repository)
        {
            _cityManager = cityManager;
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _cityRepository = cityRepository;
            _attachmentManager = attachmentManager;
            _cityTranslationRepository = cityTranslationRepository;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<CityDetailsDto> GetAsync(EntityDto<int> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<LiteCityDto>> GetAllAsync(PagedCityResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        public override async Task<CityDetailsDto> CreateAsync(CreateCityDto input)
        {
            var country = await _countryManager.GetLiteEntityByIdAsync(input.CountryId);

            if (country is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Country));
            }

            var Translation = ObjectMapper.Map<List<CityTranslation>>(input.Translations);
            if (await _cityManager.CheckIfCityIsExist(Translation))
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectIsAlreadyExist, Tokens.City));
            }

            var city = ObjectMapper.Map<City>(input);
            city.IsActive = true;
            city.CreationTime = DateTime.UtcNow;

            await Repository.InsertAsync(city);
            await CurrentUnitOfWork.SaveChangesAsync();

            if (input.AttachmentId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.City, city.Id);
            }

            await _cacheManager.GetCache(CacheName_GetCities).ClearAsync();

            return MapToEntityDto(city);
        }
        //rebuild
        public override async Task<CityDetailsDto> UpdateAsync(UpdateCityDto input)
        {
            var city = await _cityManager.GetCityByIdAsync(input.Id);

            if (city is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.City));
            }

            if (city.Country is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Country));
            }

            city.Translations.Clear();
            MapToEntity(input, city);

            var oldAttachment = await _attachmentManager.GetElementByRefAsync(city.Id, AttachmentRefType.City);

            if (input.AttachmentId == 0 && oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }
            else if (input.AttachmentId != 0 && oldAttachment is not null)
            {
                if (oldAttachment.Id != input.AttachmentId)
                {
                    await _attachmentManager.DeleteRefIdAsync(oldAttachment);
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                     input.AttachmentId, AttachmentRefType.City, city.Id);
                }
            }
            else if (input.AttachmentId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.City, city.Id);
            }

            await _cityRepository.UpdateAsync(city);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            await _cacheManager.GetCache(CacheName_GetCities).ClearAsync();

            return MapToEntityDto(city);

        }

        public async Task<CityDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
        {
            var city = await _cityManager.GetLiteEntityByIdAsync(input.Id);
            city.IsActive = !city.IsActive;
            await _cityRepository.UpdateAsync(city);
            await _cacheManager.GetCache(CacheName_GetCities).ClearAsync();
            return MapToEntityDto(city);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var city = await _cityManager.GetEntityByIdAsync(input.Id);

            if (city is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.City));
            }

            if (city.Regions.Count > 0)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectCantBeDelete, Tokens.Region));
            }


            foreach (var translation in city.Translations.ToList())
            {
                await _cityTranslationRepository.DeleteAsync(translation);
                city.Translations.Remove(translation);
            }

            var oldAttachment = await _attachmentManager.GetElementByRefAsync(city.Id, AttachmentRefType.City);

            if (oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }

            await _cityRepository.DeleteAsync(input.Id);
            await _cacheManager.GetCache(CacheName_GetCities).ClearAsync();
        }






        private async Task<CityDetailsDto> GetFromDatabase(EntityDto<int> input)
        {
            var city = await _cityManager.GetEntityByIdAsync(input.Id);

            var cityDetailsDto = MapToEntityDto(city);
            var attachment = await _attachmentManager.GetElementByRefAsync(cityDetailsDto.Id, AttachmentRefType.City);
            if (attachment is not null)
            {
                cityDetailsDto.Attachment = new LiteAttachmentDto
                {
                    Id = attachment.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                };
            }
            return cityDetailsDto;
        }

        private async Task<PagedResultDto<LiteCityDto>> GetAllFromDatabase(PagedCityResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);
            foreach (var item in result.Items)
            {
                var attachment = await _attachmentManager.GetElementByRefAsync(item.Id, AttachmentRefType.City);
                if (attachment is not null)
                {
                    item.Attachment = new LiteAttachmentDto
                    {
                        Id = attachment.Id,
                        Url = _attachmentManager.GetUrl(attachment),
                        LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                    };
                }
            }

            return result;
        }




        protected override IQueryable<City> CreateFilteredQuery(PagedCityResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.Translations);
            data = data.Include(x => x.Country).ThenInclude(x => x.Translations);

            if (input.isActive.HasValue)
                data = data.Where(x => x.IsActive == input.isActive.Value);

            if (!input.ForWanted.HasValue)
                data = data.Include(x => x.Regions).ThenInclude(x => x.Translations);
            if (input.ForWanted.HasValue)
            {
                if (input.ForWanted.Value == false)
                {
                    data = data.Include(x => x.Regions).ThenInclude(x => x.Translations);
                }
            }
            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Name.Contains(input.Keyword)).Any());

            if (input.CountryId.HasValue)
                data = data.Where(x => x.CountryId == input.CountryId);
            return data;
        }

        protected override IQueryable<City> ApplySorting(IQueryable<City> query, PagedCityResultRequestDto input)
        {
            //  var userLanguageHeader = _httpContextAccessor.HttpContext.Request.Headers.AcceptLanguage.ToString();
            //return query
            //.Include(x => x.Translations)
            //.OrderBy(x => x.Translations.Where(x => x.Language.Contains(userLanguageHeader)).OrderBy(t => t.Name).FirstOrDefault().Name);
            return query.OrderBy(r => r.Id);
        }
    }
}
