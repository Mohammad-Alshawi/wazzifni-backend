using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Configuration;
using Wazzifni.Domain.MobileApps;
using Wazzifni.Domain.MobileApps.Dtos;
using Wazzifni.Localization.SourceFiles;


namespace KeyFinder.MobileApps;

public class MobileAppAppService : ApplicationService, IMobileAppAppService
{
    private readonly IRepository<MobileApp, int> _repository;
    private readonly IRepository<AuditLog, long> _auditLogRepository;
    private readonly IMapper _mapper;
    public MobileAppAppService(IRepository<MobileApp, int> repository,
    IRepository<AuditLog, long> auditLogRepository,
    IMapper mapper)
    {
        _repository = repository;
        _auditLogRepository = auditLogRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<LiteMobileAppDto>> GetAllAsync(PagedMobileAppResultRequestDto input)
    {
        var query = _repository.GetAll();

        query = ApplyFiltering(query, input);
        query = ApplySorting(query, input);

        var totalCount = await query.CountAsync();

        var pagedQuery = ApplyPaging(query, input);

        var items = await pagedQuery
            .Select(x => _mapper.Map<LiteMobileAppDto>(x))
            .ToListAsync();

        return new PagedResultDto<LiteMobileAppDto>(totalCount, items);
    }
    public async Task<MobileAppDetailsDto> CreateAsync(CreateMobileAppDto input)
    {
        var apkBuildToInsert = ObjectMapper.Map<MobileApp>(input);
        apkBuildToInsert.UpdateOptions = UpdateOptions.Nothing;
        if (apkBuildToInsert.CreationTime == default) apkBuildToInsert.CreationTime = Clock.Now;
        apkBuildToInsert.Id = await _repository.InsertAndGetIdAsync(apkBuildToInsert);
        return ObjectMapper.Map<MobileAppDetailsDto>(apkBuildToInsert);
    }
    public async Task<MobileAppDetailsDto> UpdateAsync(UpdateMobileAppDto input)
    {
        var apk = await _repository.GetAsync(input.Id) ??
            throw new UserFriendlyException(404, Exceptions.ObjectWasNotFound);
        var updated = apk.UpdateOptions;
        _mapper.Map(input, apk);
        apk.UpdateOptions = updated;
        return ObjectMapper.Map<MobileAppDetailsDto>(updated);
    }

    //[AbpAuthorize(PermissionNames.ApkBuild)]


    public async Task<OutputBooleanStatuesDto> ChangeUpdateOptionsForApk(InputApkNuildStatuesDto input)
    {
        var apk = await _repository.GetAsync(input.Id);
        if (apk is null)
            throw new UserFriendlyException(404, Exceptions.ObjectWasNotFound);
        apk.UpdateOptions = input.UpdateOptions;
        await _repository.UpdateAsync(apk);
        await UnitOfWorkManager.Current.SaveChangesAsync();
        return new OutputBooleanStatuesDto()
        {
            BooleanStatues = true
        };

    }


    private IQueryable<MobileApp> ApplyFiltering(IQueryable<MobileApp> query, PagedMobileAppResultRequestDto input)
    {

        if (input.AppType.HasValue)
            query = query.Where(x => x.AppType == input.AppType.Value);
        if (input.SystemType.HasValue)
            query = query.Where(x => x.SystemType == input.SystemType.Value);
        if (input.UpdateOptions.HasValue)
            query = query.Where(x => x.UpdateOptions == input.UpdateOptions.Value);


        return query;
    }

    private IQueryable<MobileApp> ApplySorting(IQueryable<MobileApp> query, PagedMobileAppResultRequestDto input)
    {

        query = query.OrderByDescending(x => x.CreationTime);
        return query;
    }

    private IQueryable<MobileApp> ApplyPaging(IQueryable<MobileApp> query, PagedMobileAppResultRequestDto input)
    {
        return query.Skip(input.SkipCount).Take(input.MaxResultCount);
    }


    public async Task<MobileAppDetailsDto> CheckIfAppNeedToUpdate(InputApkBuildStatuesDto input)
    {
        var apk = await _repository.GetAll()
            .Where(x => x.AppType == input.AppType && x.VersionCode == input.VersionCode && x.SystemType == input.SystemType)
            .FirstOrDefaultAsync();

        var result = new MobileAppDetailsDto();
        if (apk is not null)
        {
            result = _mapper.Map<MobileAppDetailsDto>(apk);

            result.UpdateOptions = apk.UpdateOptions;
            result.ApkIsNotFound = false;

            result.MobileLinks.IosLinkForBasic = input.AppType is AppTypes.Basic ?
                SettingManager.GetSettingValue(AppSettingNames.IosLinkForBasic) :
                SettingManager.GetSettingValue(AppSettingNames.IosLinkForBusiness);

            result.MobileLinks.AndroidLinkForBasic = input.AppType is AppTypes.Basic ?
                SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBasic) :
                SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBusiness);

            return result;
        }
        else
            return new MobileAppDetailsDto()
            {
                UpdateOptions = UpdateOptions.Nothing,
                ApkIsNotFound = true,
            };


    }


    public async Task<MobileAppDetailsDto> GetAsync(EntityDto<int> input)
    {
        var apk = await _repository.GetAll()
                        .Where(x => x.Id == input.Id)
                        .FirstOrDefaultAsync();
        if (apk is null)
        {
            throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, typeof(MobileApp)));
        }

        var result = _mapper.Map<MobileAppDetailsDto>(apk);

        result.MobileLinks.AndroidLinkForBasic = SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBasic);
        result.MobileLinks.IosLinkForBasic = SettingManager.GetSettingValue(AppSettingNames.IosLinkForBasic);
        result.MobileLinks.AndroidLinkForBusiness = SettingManager.GetSettingValue(AppSettingNames.AndroidLinkForBusiness);
        result.MobileLinks.IosLinkForBusiness = SettingManager.GetSettingValue(AppSettingNames.IosLinkForBusiness);

        return result;


    }


    public async Task DeleteAsync(EntityDto<int> input)
    {
        var apk = await _repository.GetAll()
                        .Where(x => x.Id == input.Id)
                        .FirstOrDefaultAsync();
        if (apk is null)
        {
            throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, typeof(MobileApp)));
        }
        await _repository.DeleteAsync(apk);

    }


}
