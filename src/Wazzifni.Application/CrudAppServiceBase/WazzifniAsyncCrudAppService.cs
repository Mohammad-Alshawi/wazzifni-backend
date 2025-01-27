using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.CrudAppServiceBase;


public abstract class WazzifniAsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TLiteEntityDto, TGetAllInput, TCreateInput, TUpdateInput> :
    WazzifniCrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TLiteEntityDto, TGetAllInput, TCreateInput, TUpdateInput>,
    IWazzifniAsyncCrudAppService<TEntityDto, TPrimaryKey, TLiteEntityDto, TGetAllInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TPrimaryKey>
    where TEntityDto : IEntityDto<TPrimaryKey>
    where TLiteEntityDto : IEntityDto<TPrimaryKey>
    where TUpdateInput : IEntityDto<TPrimaryKey>
{
    public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

    protected WazzifniAsyncCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
        : base(repository)
    {
        AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        LocalizationSourceName = WazzifniConsts.LocalizationSourceName;
    }

    public virtual async Task<TEntityDto> GetAsync(EntityDto<TPrimaryKey> input)
    {
        var entity = await GetEntityByIdAsync(input.Id);
        return MapToEntityDto(entity);
    }

    public virtual async Task<PagedResultDto<TLiteEntityDto>> GetAllAsync(TGetAllInput input)
    {
        var query = CreateFilteredQuery(input);
        var str = query.ToQueryString();
        var totalCount = await AsyncQueryableExecuter.CountAsync(query);
        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);
        var entities = await AsyncQueryableExecuter.ToListAsync(query);
        return new PagedResultDto<TLiteEntityDto>(
            totalCount,
            entities.Select(MapToLiteEntityDto).ToList()
        );
    }

    public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
    {
        var entity = MapToEntity(input);

        await Repository.InsertAsync(entity);
        await CurrentUnitOfWork.SaveChangesAsync();

        return MapToEntityDto(entity);
    }

    public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
    {
        var entity = await GetEntityByIdAsync(input.Id);

        MapToEntity(input, entity);
        await CurrentUnitOfWork.SaveChangesAsync();

        return MapToEntityDto(entity);
    }

    public virtual async Task DeleteAsync(EntityDto<TPrimaryKey> input)
    {
        await Repository.DeleteAsync(input.Id);
    }

    protected virtual Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
    {
        return Repository.GetAsync(id);
    }

}
