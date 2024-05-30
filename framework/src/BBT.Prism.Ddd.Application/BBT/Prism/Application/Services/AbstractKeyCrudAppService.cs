using System;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Mapper;

namespace BBT.Prism.Application.Services;

public abstract class AbstractKeyCrudAppService<TEntity, TEntityDto, TKey>
    : AbstractKeyCrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyCrudAppService(IServiceProvider serviceProvider, IRepository<TEntity> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class AbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput>
    : AbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto, TEntityDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyCrudAppService(IServiceProvider serviceProvider, IRepository<TEntity> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class AbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
    : AbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
    where TEntity : class, IEntity
{
    protected AbstractKeyCrudAppService(IServiceProvider serviceProvider, IRepository<TEntity> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class AbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    : AbstractKeyCrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity
{
    protected AbstractKeyCrudAppService(IServiceProvider serviceProvider, IRepository<TEntity> repository)
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

public abstract class AbstractKeyCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    : AbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>,
        ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity
{
    protected IRepository<TEntity> Repository { get; }

    protected AbstractKeyCrudAppService(IServiceProvider serviceProvider, IRepository<TEntity> repository)
        : base(serviceProvider, repository)
    {
        Repository = repository;
    }

    public virtual async Task<TGetOutputDto> CreateAsync(TCreateInput input)
    {
        var entity = await MapToEntityAsync(input);
        await Repository.InsertAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
    {
        var entity = await GetEntityByIdAsync(id);
        await MapToEntityAsync(input, entity);
        await Repository.UpdateAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        await DeleteByIdAsync(id);
    }

    protected abstract Task DeleteByIdAsync(TKey id);

    /// <summary>
    /// Maps <typeparamref name="TCreateInput"/> to <typeparamref name="TEntity"/> to create a new entity.
    /// It uses <see cref="MapToEntity(TCreateInput)"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToEntity(TCreateInput)"/>
    /// </summary>
    protected virtual Task<TEntity> MapToEntityAsync(TCreateInput createInput)
    {
        return Task.FromResult(MapToEntity(createInput));
    }

    /// <summary>
    /// Maps <typeparamref name="TCreateInput"/> to <typeparamref name="TEntity"/> to create a new entity.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TEntity MapToEntity(TCreateInput createInput)
    {
        var entity = ObjectMapper.Map<TCreateInput, TEntity>(createInput);
        SetIdForGuids(entity);
        return entity;
    }

    /// <summary>
    /// Sets Id value for the entity if <typeparamref name="TKey"/> is <see cref="Guid"/>.
    /// It's used while creating a new entity.
    /// </summary>
    protected virtual void SetIdForGuids(TEntity entity)
    {
        if (entity is IEntity<Guid> entityWithGuidId && entityWithGuidId.Id == Guid.Empty)
        {
            EntityHelper.TrySetId(
                entityWithGuidId,
                () => GuidGenerator.Create(),
                true
            );
        }
    }

    /// <summary>
    /// Maps <typeparamref name="TUpdateInput"/> to <typeparamref name="TEntity"/> to update the entity.
    /// It uses <see cref="MapToEntity(TUpdateInput, TEntity)"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToEntity(TUpdateInput, TEntity)"/>
    /// </summary>
    protected virtual Task MapToEntityAsync(TUpdateInput updateInput, TEntity entity)
    {
        MapToEntity(updateInput, entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Maps <typeparamref name="TUpdateInput"/> to <typeparamref name="TEntity"/> to update the entity.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
    {
        ObjectMapper.Map(updateInput, entity);
    }
}
