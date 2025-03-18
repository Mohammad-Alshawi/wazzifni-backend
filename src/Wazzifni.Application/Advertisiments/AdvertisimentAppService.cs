using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

using System.Threading.Tasks;
using Wazzifni.Advertisiments.Dto;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Advertisiments;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Advertisiments
{
    public class AdvertisimentAppService :
        WazzifniAsyncCrudAppService<Advertisiment, AdvertisimentDetailsDto, int, LiteAdvertisimentDto, PagedAdvertisimentResultRequestDto, CreateAdvertisimentDto, UpdateAdvertisimentDto>,
        IAdvertisimentAppService
    {

        public const string CacheName_GetAdvertisements = "GET-ADVERTISEMENTS";

        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly WorkPostManager WorkPostManager;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IAdvertisimentManager _advertisimentManager;

        public AdvertisimentAppService(
            UserManager userManager,
            ICacheManager cacheManager,
            WorkPostManager WorkPostManager,
            IAttachmentManager attachmentManager,
            IRepository<Advertisiment> repository,
            IAdvertisimentManager advertisimentManager
        ) : base(repository)
        {

            _userManager = userManager;
            _cacheManager = cacheManager;
            this.WorkPostManager = WorkPostManager;
            _attachmentManager = attachmentManager;
            _advertisimentManager = advertisimentManager;
        }


        [HttpPut, AbpAuthorize(PermissionNames.Advertisements_CUD)]
        public override async Task<AdvertisimentDetailsDto> UpdateAsync(UpdateAdvertisimentDto input)
        {
            var Advertisiment = await _advertisimentManager.GetEntityAsync(input.Id);

            if (Advertisiment is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Advertisiment));
            }


            MapToEntity(input, Advertisiment);
            Advertisiment.LastModificationTime = DateTime.Now;
            var oldAttachment = await _attachmentManager.GetElementByRefAsync(Advertisiment.Id, AttachmentRefType.Advertisiment);
            if (oldAttachment.Id != input.AttachmentId)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.Advertisiment, input.Id);
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }
            await Repository.UpdateAsync(Advertisiment);


            var AdvertisimentDto = MapToEntityDto(Advertisiment);
            await _cacheManager.GetCache(CacheName_GetAdvertisements).ClearAsync();
            return AdvertisimentDto;
        }




        [HttpPost, AbpAuthorize(PermissionNames.Advertisements_CUD)]
        public override async Task<AdvertisimentDetailsDto> CreateAsync(CreateAdvertisimentDto input)
        {
            var advertisiment = ObjectMapper.Map<Advertisiment>(input);
            advertisiment.CreatorUserId = AbpSession.UserId.Value;
            advertisiment.CreationTime = DateTime.Now;
            var advertismentId = await Repository.InsertAndGetIdAsync(advertisiment);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.Advertisiment, advertismentId);

            await _cacheManager.GetCache(CacheName_GetAdvertisements).ClearAsync();
            return MapToEntityDto(advertisiment);

        }



        [HttpGet, AbpAllowAnonymous]
        public override async Task<PagedResultDto<LiteAdvertisimentDto>> GetAllAsync(PagedAdvertisimentResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<AdvertisimentDetailsDto> GetAsync(EntityDto<int> input)
        {
            if (await _userManager.IsAdminSession()) return await GetFromDatabase(input);

            return await _cacheManager
                .GetCache<string, AdvertisimentDetailsDto>(CacheName_GetAdvertisements)
                .GetAsync(input.GetLocalizedId(), async key => await GetFromDatabase(input));
        }



        private async Task<PagedResultDto<LiteAdvertisimentDto>> GetAllFromDatabase(PagedAdvertisimentResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            foreach (var item in result.Items)
            {
                var user = await _userManager.GetUserByIdAsync((long)item.CreatorUserId);
                item.CreatorUserName = user.FullName;

                var attachment = await _attachmentManager.GetElementByRefAsync(item.Id, AttachmentRefType.Advertisiment);
                if (attachment != null)
                {
                    item.Attachment = new LiteAttachmentDto
                    {
                        Id = attachment.Id,
                        Url = _attachmentManager.GetUrl(attachment),
                        LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                    };
                }
                if (item.WorkPostId.HasValue)
                {
                    if (item.WorkPostId.Value != 0)
                    {
                        var WorkPost = await WorkPostManager.GetEntityByIdAsync(item.WorkPostId.Value);
                        item.IsForWorkPost = true;
                    }
                }
            }
            return result;
        }

        private async Task<AdvertisimentDetailsDto> GetFromDatabase(EntityDto<int> input)
        {
            var Advertisiment = await Repository.GetAsync(input.Id);
            if (Advertisiment is null)
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Advertisiment));
            var user = await _userManager.GetUserByIdAsync((long)Advertisiment.CreatorUserId);
            var advertisimentDto = MapToEntityDto(Advertisiment);
            advertisimentDto.CreatorUserName = user.FullName;

            var attachment = await _attachmentManager.GetElementByRefAsync(advertisimentDto.Id, AttachmentRefType.Advertisiment);
            if (attachment != null)
            {
                advertisimentDto.Attachment = new LiteAttachmentDto
                {
                    Id = attachment.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                };
            }
            if (advertisimentDto.WorkPostId.HasValue)
            {
                if (advertisimentDto.WorkPostId.Value != 0)
                {
                    var WorkPost = await WorkPostManager.GetEntityByIdAsync(advertisimentDto.WorkPostId.Value);

                    advertisimentDto.IsForWorkPost = true;
                }
            }
            return advertisimentDto;
        }





        protected override IQueryable<Advertisiment> ApplySorting(IQueryable<Advertisiment> query, PagedAdvertisimentResultRequestDto input)
        {
            return query.OrderBy(r => r.CreationTime);
        }

        protected override IQueryable<Advertisiment> CreateFilteredQuery(PagedAdvertisimentResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            if (input.Type.HasValue)
            {
                data = data.Where(x => x.Type == input.Type);
            }
            if (input.WorkPostId.HasValue)
            {
                data = data.Where(x => x.WorkPostId == input.WorkPostId);
            }

            return data;
        }

        [AbpAuthorize(PermissionNames.Advertisements_CUD)]

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            var Advertisiment = await _advertisimentManager.GetEntityAsync(input.Id);

            if (Advertisiment is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Advertisiment));
            }

            var oldAttachment = await _attachmentManager.GetElementByRefAsync(Advertisiment.Id, AttachmentRefType.Advertisiment);

            if (oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }

            await Repository.DeleteAsync(input.Id);
            await _cacheManager.GetCache(CacheName_GetAdvertisements).ClearAsync();
        }

    }
}


