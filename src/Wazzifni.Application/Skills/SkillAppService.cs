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
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.Skills;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Skills.Dto;

namespace Wazzifni.Skills
{

    public class SkillAppService :
        WazzifniAsyncCrudAppService<Skill, SkillDetailsDto, int, LiteSkillDto, PagedSkillResultRequestDto, CreateSkillDto, UpdateSkillDto>,
        ISkillAppService
    {

        public const string CacheName_GetCities = "GET-CITIES";

        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<Skill> _SkillRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IRepository<SkillTranslation> _SkillTranslationRepository;


        public SkillAppService(
            UserManager userManager,
            ICacheManager cacheManager,
            IRepository<Skill> repository,
            CountryManager countryManager,
            IRepository<Skill> SkillRepository,
            IAttachmentManager attachmentManager,
            IRepository<SkillTranslation> SkillTranslationRepository
        ) : base(repository)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _SkillRepository = SkillRepository;
            _attachmentManager = attachmentManager;
            _SkillTranslationRepository = SkillTranslationRepository;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<SkillDetailsDto> GetAsync(EntityDto<int> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<LiteSkillDto>> GetAllAsync(PagedSkillResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        [AbpAuthorize(PermissionNames.Skill_Create)]

        public override async Task<SkillDetailsDto> CreateAsync(CreateSkillDto input)
        {

            var Skill = ObjectMapper.Map<Skill>(input);
            Skill.IsActive = true;
            await Repository.InsertAsync(Skill);
            await CurrentUnitOfWork.SaveChangesAsync();
            return MapToEntityDto(Skill);
        }
       
        [AbpAuthorize(PermissionNames.Skill_Update)]

        public override async Task<SkillDetailsDto> UpdateAsync(UpdateSkillDto input)
        {
            var Skill = await _SkillRepository.GetAll().Include(c => c.Translations).Where(SL => SL.Id == input.Id).FirstOrDefaultAsync(); ;

            if (Skill is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.Skill"));
            }

            Skill.Translations.Clear();
            MapToEntity(input, Skill);

            await _SkillRepository.UpdateAsync(Skill);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return MapToEntityDto(Skill);

        }

        [AbpAuthorize(PermissionNames.Skill_Update)]
        public async Task<SkillDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
        {
            var Skill = await _SkillRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();
            Skill.IsActive = !Skill.IsActive;
            await _SkillRepository.UpdateAsync(Skill);
            await _cacheManager.GetCache(CacheName_GetCities).ClearAsync();
            return MapToEntityDto(Skill);
        }


        [AbpAuthorize(PermissionNames.Skill_Delete)]

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var Skill = await _SkillRepository.GetAll().Include(x => x.Translations).Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();

            if (Skill is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.Skill"));
            }


            foreach (var translation in Skill.Translations.ToList())
            {
                await _SkillTranslationRepository.DeleteAsync(translation);
                Skill.Translations.Remove(translation);
            }

            await _SkillRepository.DeleteAsync(input.Id);
        }






        private async Task<SkillDetailsDto> GetFromDatabase(EntityDto<int> input)
        {
            var Skill = await _SkillRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();

            var SkillDetailsDto = MapToEntityDto(Skill);

            return SkillDetailsDto;
        }

        private async Task<PagedResultDto<LiteSkillDto>> GetAllFromDatabase(PagedSkillResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            return result;
        }




        protected override IQueryable<Skill> CreateFilteredQuery(PagedSkillResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.Translations);

            if (input.IsActive.HasValue)
                data = data.Where(x => x.IsActive == input.IsActive.Value);


            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Name.Contains(input.Keyword)).Any());

            return data;
        }

        protected override IQueryable<Skill> ApplySorting(IQueryable<Skill> query, PagedSkillResultRequestDto input)
        {

            return query.OrderBy(r => r.Id);
        }
    }
}
