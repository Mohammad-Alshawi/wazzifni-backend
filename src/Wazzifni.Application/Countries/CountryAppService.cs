using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using KeyFinder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Wazzifni.Countries.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Countries;
using Wazzifni.Localization.SourceFiles;

namespace Wazzifni.Countries
{
    public class CountryAppService :
        WazzifniAsyncCrudAppService<Country, CountryDetailsDto, int, CountryDto, PagedCountryResultRequestDto, CreateCountryDto, UpdateCountryDto>,
        ICountryAppService
    {
        private readonly ICountryManager _countryManager;
        private readonly IRepository<Country> _countryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<CountryTranslation> _countryTranslationRepository;

        public CountryAppService(
            ICountryManager countryManager,
            IRepository<Country> repository,
            IRepository<Country> countryRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<CountryTranslation> countryTranslationRepository
        ) : base(repository)
        {
            _countryManager = countryManager;
            _countryRepository = countryRepository;
            _httpContextAccessor = httpContextAccessor;
            _countryTranslationRepository = countryTranslationRepository;
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<CountryDetailsDto> GetAsync(EntityDto<int> input)
        {
            var country = await _countryManager.GetEntityByIdAsync(input.Id);
            if (country is null) throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Country));
            return MapToEntityDto(country);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<CountryDto>> GetAllAsync(PagedCountryResultRequestDto input)
        {
            return await base.GetAllAsync(input);
        }


        public override async Task<CountryDetailsDto> CreateAsync(CreateCountryDto input)
        {
            var Translation = ObjectMapper.Map<List<CountryTranslation>>(input.Translations);
            if (await _countryManager.CheckIfCountryExist(Translation))
                throw new UserFriendlyException(string.Format(Exceptions.ObjectIsAlreadyExist, Tokens.Country));
            var country = ObjectMapper.Map<Country>(input);
            country.IsActive = true;
            await _countryRepository.InsertAsync(country);
            return MapToEntityDto(country);
        }

        public override async Task<CountryDetailsDto> UpdateAsync(UpdateCountryDto input)
        {
            var country = await _countryManager.GetEntityByIdAsync(input.Id);
            if (country is null)
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Country));
            country.Translations.Clear();
            MapToEntity(input, country);
            country.LastModificationTime = DateTime.UtcNow;
            await _countryRepository.UpdateAsync(country);
            return MapToEntityDto(country);

        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var country = await _countryManager.GetEntityByIdAsync(input.Id);
            if (country is null)
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Country));
            if (country.Cities.Count > 0)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectCantBeDelete, Tokens.City));
            }
            foreach (var translation in country.Translations.ToList())
            {
                await _countryTranslationRepository.DeleteAsync(translation);
                country.Translations.Remove(translation);
            }
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<CountryDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
        {
            var country = await _countryManager.GetLiteEntityByIdAsync(input.Id);
            if (country is null)
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Country));
            country.IsActive = !country.IsActive;
            country.LastModificationTime = DateTime.UtcNow;
            await _countryRepository.UpdateAsync(country);
            return MapToEntityDto(country);

        }



        protected override IQueryable<Country> CreateFilteredQuery(PagedCountryResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.Translations);
            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Name.Contains(input.Keyword)).Any() || x.DialCode.Contains(input.Keyword));
            if (input.IsActive.HasValue)
                data = data.Where(x => x.IsActive == input.IsActive.Value);
            return data;
        }

        protected override IQueryable<Country> ApplySorting(IQueryable<Country> query, PagedCountryResultRequestDto input)
        {
            var userLanguageHeader = _httpContextAccessor.HttpContext.Request.Headers.AcceptLanguage.ToString();

            return query
            .Include(x => x.Translations)
            .OrderBy(x => x.Translations.Where(x => x.Language.Contains(userLanguageHeader)).OrderBy(t => t.Name).FirstOrDefault().Name);
        }

    }
}
