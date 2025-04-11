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
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.Universities;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Universities.Dto;

namespace Wazzifni.Universitys
{

    public class UniversityAppService :
        WazzifniAsyncCrudAppService<University, UniversityDetailsDto, int, LiteUniversityDto, PagedUniversityResultRequestDto, CreateUniversityDto, UpdateUniversityDto>,
        IUniversityAppService
    {

        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<University> _UniversityRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IRepository<UniversityTranslation> _UniversityTranslationRepository;


        public UniversityAppService(
            UserManager userManager,
            ICacheManager cacheManager,
            IRepository<University> repository,
            CountryManager countryManager,
            IRepository<University> UniversityRepository,
            IAttachmentManager attachmentManager,
            IRepository<UniversityTranslation> UniversityTranslationRepository
        ) : base(repository)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _UniversityRepository = UniversityRepository;
            _attachmentManager = attachmentManager;
            _UniversityTranslationRepository = UniversityTranslationRepository;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<UniversityDetailsDto> GetAsync(EntityDto<int> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<LiteUniversityDto>> GetAllAsync(PagedUniversityResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        public override async Task<UniversityDetailsDto> CreateAsync(CreateUniversityDto input)
        {

            var University = ObjectMapper.Map<University>(input);
            University.IsActive = true;
            await Repository.InsertAsync(University);
            await CurrentUnitOfWork.SaveChangesAsync();
            return MapToEntityDto(University);
        }
        public override async Task<UniversityDetailsDto> UpdateAsync(UpdateUniversityDto input)
        {
            var University = await _UniversityRepository.GetAll().Include(c => c.Translations).Where(SL => SL.Id == input.Id).FirstOrDefaultAsync(); ;

            if (University is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.University"));
            }

            University.Translations.Clear();
            MapToEntity(input, University);

            await _UniversityRepository.UpdateAsync(University);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return MapToEntityDto(University);

        }

        public async Task<UniversityDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
        {
            var University = await _UniversityRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();
            University.IsActive = !University.IsActive;
            await _UniversityRepository.UpdateAsync(University);
            return MapToEntityDto(University);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var University = await _UniversityRepository.GetAll().Include(x => x.Translations).Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();

            if (University is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.University"));
            }


            foreach (var translation in University.Translations.ToList())
            {
                await _UniversityTranslationRepository.DeleteAsync(translation);
                University.Translations.Remove(translation);
            }

            await _UniversityRepository.DeleteAsync(input.Id);
        }






        private async Task<UniversityDetailsDto> GetFromDatabase(EntityDto<int> input)
        {
            var University = await _UniversityRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();

            var UniversityDetailsDto = MapToEntityDto(University);

            return UniversityDetailsDto;
        }

        private async Task<PagedResultDto<LiteUniversityDto>> GetAllFromDatabase(PagedUniversityResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            return result;
        }




        protected override IQueryable<University> CreateFilteredQuery(PagedUniversityResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.Translations);

            if (input.IsActive.HasValue)
                data = data.Where(x => x.IsActive == input.IsActive.Value);


            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Name.Contains(input.Keyword)).Any());

            return data;
        }

        protected override IQueryable<University> ApplySorting(IQueryable<University> query, PagedUniversityResultRequestDto input)
        {

            return query.OrderBy(r => r.Id);
        }
    }
}
