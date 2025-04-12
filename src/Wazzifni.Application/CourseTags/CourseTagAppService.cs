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
using Wazzifni.Domain.CourseTags;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.CourseTags.Dto;

namespace Wazzifni.CourseTags
{

    public class CourseTagAppService :
        WazzifniAsyncCrudAppService<CourseTag, CourseTagDetailsDto, int, LiteCourseTagDto, PagedCourseTagResultRequestDto, CreateCourseTagDto, UpdateCourseTagDto>,
        ICourseTagAppService
    {

        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<CourseTag> _CourseTagRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IRepository<CourseTagTranslation> _CourseTagTranslationRepository;


        public CourseTagAppService(
            UserManager userManager,
            ICacheManager cacheManager,
            IRepository<CourseTag> repository,
            CountryManager countryManager,
            IRepository<CourseTag> CourseTagRepository,
            IAttachmentManager attachmentManager,
            IRepository<CourseTagTranslation> CourseTagTranslationRepository
        ) : base(repository)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _CourseTagRepository = CourseTagRepository;
            _attachmentManager = attachmentManager;
            _CourseTagTranslationRepository = CourseTagTranslationRepository;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<CourseTagDetailsDto> GetAsync(EntityDto<int> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<LiteCourseTagDto>> GetAllAsync(PagedCourseTagResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        public override async Task<CourseTagDetailsDto> CreateAsync(CreateCourseTagDto input)
        {

            var CourseTag = ObjectMapper.Map<CourseTag>(input);
            CourseTag.IsActive = true;
            await Repository.InsertAsync(CourseTag);
            await CurrentUnitOfWork.SaveChangesAsync();
            return MapToEntityDto(CourseTag);
        }
        public override async Task<CourseTagDetailsDto> UpdateAsync(UpdateCourseTagDto input)
        {
            var CourseTag = await _CourseTagRepository.GetAll().Include(c => c.Translations).Where(SL => SL.Id == input.Id).FirstOrDefaultAsync(); ;

            if (CourseTag is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.CourseTag"));
            }

            CourseTag.Translations.Clear();
            MapToEntity(input, CourseTag);

            await _CourseTagRepository.UpdateAsync(CourseTag);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return MapToEntityDto(CourseTag);

        }

        public async Task<CourseTagDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
        {
            var CourseTag = await _CourseTagRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();
            CourseTag.IsActive = !CourseTag.IsActive;
            await _CourseTagRepository.UpdateAsync(CourseTag);
            return MapToEntityDto(CourseTag);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var CourseTag = await _CourseTagRepository.GetAll().Include(x => x.Translations).Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();

            if (CourseTag is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.CourseTag"));
            }


            foreach (var translation in CourseTag.Translations.ToList())
            {
                await _CourseTagTranslationRepository.DeleteAsync(translation);
                CourseTag.Translations.Remove(translation);
            }

            await _CourseTagRepository.DeleteAsync(input.Id);
        }






        private async Task<CourseTagDetailsDto> GetFromDatabase(EntityDto<int> input)
        {
            var CourseTag = await _CourseTagRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();

            var CourseTagDetailsDto = MapToEntityDto(CourseTag);

            return CourseTagDetailsDto;
        }

        private async Task<PagedResultDto<LiteCourseTagDto>> GetAllFromDatabase(PagedCourseTagResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            return result;
        }




        protected override IQueryable<CourseTag> CreateFilteredQuery(PagedCourseTagResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.Translations);

            if (input.IsActive.HasValue)
                data = data.Where(x => x.IsActive == input.IsActive.Value);


            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Name.Contains(input.Keyword)).Any());

            return data;
        }

        protected override IQueryable<CourseTag> ApplySorting(IQueryable<CourseTag> query, PagedCourseTagResultRequestDto input)
        {

            return query.OrderBy(r => r.Id);
        }
    }
}
