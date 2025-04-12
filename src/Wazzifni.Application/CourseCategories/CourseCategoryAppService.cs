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
using Wazzifni.CourseCategories.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.CourseCategories
{

    public class CourseCategoryAppService :
        WazzifniAsyncCrudAppService<CourseCategory, CourseCategoryDetailsDto, int, LiteCourseCategoryDto, PagedCourseCategoryResultRequestDto, CreateCourseCategoryDto, UpdateCourseCategoryDto>,
        ICourseCategoryAppService
    {


        private readonly UserManager _userManager;
        private readonly CourseCategoryManager _CourseCategoryManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<CourseCategory> _CourseCategoryRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IRepository<CourseCategoryTranslation> _CourseCategoryTranslationRepository;


        public CourseCategoryAppService(
            CourseCategoryManager CourseCategoryManager,
            UserManager userManager,
            ICacheManager cacheManager,
            IRepository<CourseCategory> repository,
            CountryManager countryManager,
            IRepository<CourseCategory> CourseCategoryRepository,
            IAttachmentManager attachmentManager,
            IRepository<CourseCategoryTranslation> CourseCategoryTranslationRepository
        ) : base(repository)
        {
            _CourseCategoryManager = CourseCategoryManager;
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _CourseCategoryRepository = CourseCategoryRepository;
            _attachmentManager = attachmentManager;
            _CourseCategoryTranslationRepository = CourseCategoryTranslationRepository;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<CourseCategoryDetailsDto> GetAsync(EntityDto<int> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<LiteCourseCategoryDto>> GetAllAsync(PagedCourseCategoryResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        public override async Task<CourseCategoryDetailsDto> CreateAsync(CreateCourseCategoryDto input)
        {
        

            var Translation = ObjectMapper.Map<List<CourseCategoryTranslation>>(input.Translations);
            if (await _CourseCategoryManager.CheckIfCourseCategoryIsExist(Translation))
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectIsAlreadyExist, Tokens.CourseCategory));
            }

            var CourseCategory = ObjectMapper.Map<CourseCategory>(input);
            CourseCategory.IsActive = true;
            CourseCategory.CreationTime = DateTime.UtcNow;

            await Repository.InsertAsync(CourseCategory);
            await CurrentUnitOfWork.SaveChangesAsync();

            if (input.AttachmentId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.CourseCategory, CourseCategory.Id);
            }


            return MapToEntityDto(CourseCategory);
        }
        //rebuild
        public override async Task<CourseCategoryDetailsDto> UpdateAsync(UpdateCourseCategoryDto input)
        {
            var CourseCategory = await _CourseCategoryManager.GetCourseCategoryByIdAsync(input.Id);

            if (CourseCategory is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.CourseCategory));
            }

           

            CourseCategory.Translations.Clear();
            MapToEntity(input, CourseCategory);

            var oldAttachment = await _attachmentManager.GetElementByRefAsync(CourseCategory.Id, AttachmentRefType.CourseCategory);

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
                     input.AttachmentId, AttachmentRefType.CourseCategory, CourseCategory.Id);
                }
            }
            else if (input.AttachmentId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.CourseCategory, CourseCategory.Id);
            }

            await _CourseCategoryRepository.UpdateAsync(CourseCategory);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return MapToEntityDto(CourseCategory);

        }

        public async Task<CourseCategoryDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
        {
            var CourseCategory = await _CourseCategoryManager.GetLiteEntityByIdAsync(input.Id);
            CourseCategory.IsActive = !CourseCategory.IsActive;
            await _CourseCategoryRepository.UpdateAsync(CourseCategory);
            return MapToEntityDto(CourseCategory);
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var CourseCategory = await _CourseCategoryManager.GetEntityByIdAsync(input.Id);

            if (CourseCategory is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.CourseCategory));
            }

         
            foreach (var translation in CourseCategory.Translations.ToList())
            {
                await _CourseCategoryTranslationRepository.DeleteAsync(translation);
                CourseCategory.Translations.Remove(translation);
            }

            var oldAttachment = await _attachmentManager.GetElementByRefAsync(CourseCategory.Id, AttachmentRefType.CourseCategory);

            if (oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }

            await _CourseCategoryRepository.DeleteAsync(input.Id);
        }






        private async Task<CourseCategoryDetailsDto> GetFromDatabase(EntityDto<int> input)
        {
            var CourseCategory = await _CourseCategoryManager.GetEntityByIdAsync(input.Id);

            var CourseCategoryDetailsDto = MapToEntityDto(CourseCategory);
            var attachment = await _attachmentManager.GetElementByRefAsync(CourseCategoryDetailsDto.Id, AttachmentRefType.CourseCategory);
            if (attachment is not null)
            {
                CourseCategoryDetailsDto.Attachment = new LiteAttachmentDto
                {
                    Id = attachment.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                };
            }
            return CourseCategoryDetailsDto;
        }

        private async Task<PagedResultDto<LiteCourseCategoryDto>> GetAllFromDatabase(PagedCourseCategoryResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);
            foreach (var item in result.Items)
            {
                var attachment = await _attachmentManager.GetElementByRefAsync(item.Id, AttachmentRefType.CourseCategory);
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




        protected override IQueryable<CourseCategory> CreateFilteredQuery(PagedCourseCategoryResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.Translations);

            if (input.isActive.HasValue)
                data = data.Where(x => x.IsActive == input.isActive.Value);

           
            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Translations.Where(x => x.Name.Contains(input.Keyword)).Any());

            
            return data;
        }

        protected override IQueryable<CourseCategory> ApplySorting(IQueryable<CourseCategory> query, PagedCourseCategoryResultRequestDto input)
        {
            
            return query.OrderBy(r => r.Id);
        }
    }
}
