using System;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Application.Dtos;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Repositories;

namespace BBT.Prism.Application.Services;

public abstract class ReadOnlyAppService<TEntity, TEntityDto, TKey>
    : ReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity<TKey>
{
    protected ReadOnlyAppService(IServiceProvider serviceProvider, IReadOnlyRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class ReadOnlyAppService<TEntity, TEntityDto, TKey, TGetListInput>
    : ReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput>
    where TEntity : class, IEntity<TKey>
{
    protected ReadOnlyAppService(IServiceProvider serviceProvider, IReadOnlyRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {

    }
}

public abstract class ReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    : AbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    where TEntity : class, IEntity<TKey>
{
    protected IReadOnlyRepository<TEntity, TKey> Repository { get; }

    protected ReadOnlyAppService(IServiceProvider serviceProvider, IReadOnlyRepository<TEntity, TKey> repository)
        : base(serviceProvider, repository)
    {
        Repository = repository;
    }

    protected override async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await Repository.GetAsync(id);
    }

    protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<ICreationAuditedObject>())
        {
            return query.OrderByDescending(e => ((ICreationAuditedObject)e).CreatedAt);
        }
        else
        {
            return query.OrderByDescending(e => e.Id);
        }
    }
}
