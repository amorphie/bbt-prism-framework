using System;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Repositories;

namespace BBT.Prism.Application.Services;

public abstract class CrudAppService<TEntity, TEntityDto, TKey>
    : CrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity<TKey>
{
    protected CrudAppService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class CrudAppService<TEntity, TEntityDto, TKey, TGetListInput>
    : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>
    where TEntity : class, IEntity<TKey>
{
    protected CrudAppService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
    : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
    where TEntity : class, IEntity<TKey>
{
    protected CrudAppService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    : CrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TKey>
{
    protected CrudAppService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {

    }

    protected override Task<TEntityDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return MapToGetOutputDtoAsync(entity);
    }

    protected override TEntityDto MapToGetListOutputDto(TEntity entity)
    {
        return MapToGetOutputDto(entity);
    }
}

public abstract class CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    : AbstractKeyCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TKey>
{
    protected new IRepository<TEntity, TKey> Repository { get; }

    protected CrudAppService(IServiceProvider serviceProvider, IRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {
        Repository = repository;
    }

    protected override async Task DeleteByIdAsync(TKey id)
    {
        await Repository.DeleteAsync(id);
    }

    protected override async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await Repository.GetAsync(id);
    }

    protected override void MapToEntity(TUpdateInput updateInput, TEntity entity)
    {
        if (updateInput is IEntityDto<TKey> entityDto)
        {
            entityDto.Id = entity.Id;
        }

        base.MapToEntity(updateInput, entity);
    }

    protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<IHasCreatedAt>())
        {
            return query.OrderByDescending(e => ((IHasCreatedAt)e).CreatedAt);
        }
        else
        {
            return query.OrderByDescending(e => e.Id);
        }
    }
}
