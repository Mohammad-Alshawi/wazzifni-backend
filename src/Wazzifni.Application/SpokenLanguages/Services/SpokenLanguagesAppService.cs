using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using AutoMapper;
using KeyFinder;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.SpokenLanguages.DTOs;
using static Wazzifni.Enums.Enum;

namespace Tyoutor.SpokenLanguages.Services;

public class SpokenLanguagesAppService :
        WazzifniAsyncCrudAppService<SpokenLanguage, SpokenLanguageDetailsDto, int, LiteSpokenLanguageDto, PagedSpokenLanguageRequestResultDto, CreateSpokenLanguageDto, UpdateSpokenLanguageDto>, ISpokenLanguagesAppService
{
    private readonly IRepository<SpokenLanguage, int> _mainRepository;
    private readonly IMapper _mapper;
    private readonly IAttachmentManager _attachmentManager;

    public SpokenLanguagesAppService(IRepository<SpokenLanguage, int> mainRepository, IMapper mapper, IAttachmentManager attachmentManager) : base(mainRepository)
    {
        _mainRepository = mainRepository;
        _mapper = mapper;
        _attachmentManager = attachmentManager;
    }

    //[AbpAuthorize(PermissionNames.SpokenLanguageCUD)]
    public async Task<SpokenLanguageDetailsDto> Create(CreateSpokenLanguageDto input)
    {
        var lan = new SpokenLanguage { Name = input.Name, DisplayName = input.DisplayName, IsActive = true };
        var language = await _mainRepository.InsertAsync(lan);

        if (input.AttachmentId != 0)
        {
            await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.SpokenLanguage, language.Id);
        }
        return _mapper.Map<SpokenLanguageDetailsDto>(lan);
    }

    //[AbpAuthorize(PermissionNames.SpokenLanguageCUD)]
    public override async Task<SpokenLanguageDetailsDto> UpdateAsync(UpdateSpokenLanguageDto input)
    {
        var lan = await _mainRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();
        lan.Name = input.Name;
        lan.DisplayName = input.DisplayName;
        lan = await _mainRepository.UpdateAsync(lan);

        var oldAttachment = await _attachmentManager.GetElementByRefAsync(lan.Id, AttachmentRefType.SpokenLanguage);

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
                 input.AttachmentId, AttachmentRefType.SpokenLanguage, lan.Id);
            }
        }
        else if (input.AttachmentId != 0)
        {
            await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.SpokenLanguage, lan.Id);
        }
        return _mapper.Map<SpokenLanguageDetailsDto>(lan);
    }

    [AbpAllowAnonymous]
    public override async Task<SpokenLanguageDetailsDto> GetAsync(EntityDto<int> input)
    {
        var lan = await _mainRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();
        var result = _mapper.Map<SpokenLanguageDetailsDto>(lan); ;
        var attachment = await _attachmentManager.GetElementByRefAsync(result.Id, AttachmentRefType.SpokenLanguage);
        if (attachment is not null)
        {
            result.Icon = new LiteAttachmentDto
            {
                Id = attachment.Id,
                Url = _attachmentManager.GetUrl(attachment),
                LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
            };
        }
        return result;
    }

    [AbpAllowAnonymous]
    public override async Task<PagedResultDto<LiteSpokenLanguageDto>> GetAllAsync(PagedSpokenLanguageRequestResultDto input)
    {
        var result = await base.GetAllAsync(input);
        foreach (var item in result.Items)
        {
            var attachment = await _attachmentManager.GetElementByRefAsync(item.Id, AttachmentRefType.SpokenLanguage);
            if (attachment is not null)
            {
                item.Icon = new LiteAttachmentDto
                {
                    Id = attachment.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                };
            }
        }

        return result;
    }

    public async Task<SpokenLanguageDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input)
    {
        var SpokenLanguage = await _mainRepository.GetAll().Where(SL => SL.Id == input.Id).FirstOrDefaultAsync();
        SpokenLanguage.IsActive = !SpokenLanguage.IsActive;
        await _mainRepository.UpdateAsync(SpokenLanguage);
        return MapToEntityDto(SpokenLanguage);
    }


    protected override IQueryable<SpokenLanguage> CreateFilteredQuery(PagedSpokenLanguageRequestResultDto input)
    {
        var data = base.CreateFilteredQuery(input);


        if (input.IsActive.HasValue)
            data = data.Where(x => x.IsActive == input.IsActive.Value);


        if (!input.Keyword.IsNullOrEmpty())
            data = data.Where(x => x.Name.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword));

        return data;
    }

    protected override IQueryable<SpokenLanguage> ApplySorting(IQueryable<SpokenLanguage> query, PagedSpokenLanguageRequestResultDto input)
    {

        return query.OrderBy(r => r.Id);
    }
}
