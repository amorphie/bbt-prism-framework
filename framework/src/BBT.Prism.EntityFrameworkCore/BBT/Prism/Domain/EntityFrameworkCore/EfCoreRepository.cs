using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.EntityFrameworkCore;
using BBT.Prism.Guids;
using BBT.Prism.Timing;
using BBT.Prism.Uow;
using BBT.Prism.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Domain.EntityFrameworkCore;

public class EfCoreRepository<TDbContext, TEntity>(
    TDbContext dbContext,
    IServiceProvider serviceProvider)
    : RepositoryBase<TEntity>(serviceProvider), IEfCoreRepository<TEntity>
    where TDbContext : class, IEfCoreDbContext
    where TEntity : class, IEntity
{
    public IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();
    public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    async Task<DbContext> IEfCoreRepository<TEntity>.GetDbContextAsync()
    {
        return (await GetDbContextAsync() as DbContext)!;
    }

    protected virtual Task<TDbContext> GetDbContextAsync()
    {
        return Task.FromResult(dbContext);
    }

    Task<DbSet<TEntity>> IEfCoreRepository<TEntity>.GetDbSetAsync()
    {
        return GetDbSetAsync();
    }

    protected async Task<DbSet<TEntity>> GetDbSetAsync()
    {
        return (await GetDbContextAsync()).Set<TEntity>();
    }

    protected async Task<IDbConnection> GetDbConnectionAsync()
    {
        return (await GetDbContextAsync()).Database.GetDbConnection();
    }

    protected async Task<IDbTransaction?> GetDbTransactionAsync()
    {
        return (await GetDbContextAsync()).Database.CurrentTransaction?.GetDbTransaction();
    }

    public async override Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        CheckAndSetId(entity);

        var context = await this.GetDbContextAsync();

        var savedEntity = (await context.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
        return savedEntity;
    }

    public async override Task InsertManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }
        
        var context = await GetDbContextAsync();
        foreach (var entity in entityArray)
        {
            CheckAndSetId(entity);
        }

        await context.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);
    }

    public async override Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();

        if (context.Set<TEntity>().Local.All(e => e != entity))
        {
            context.Set<TEntity>().Attach(entity);
            context.Update(entity);
        }

        return entity;
    }

    public async override Task UpdateManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

        var context = await GetDbContextAsync();

        context.Set<TEntity>().UpdateRange(entityArray);
    }

    public async override Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();

        context.Set<TEntity>().Remove(entity);
    }

    public async override Task DeleteManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

        var context = await GetDbContextAsync();

        context.RemoveRange(entityArray.Select(x => x));
    }

    public async override Task<List<TEntity>> GetListAsync(bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).ToListAsync(cancellationToken)
            : await (await GetQueryableAsync()).ToListAsync(cancellationToken);
    }

    public async override Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).Where(predicate).ToListAsync(cancellationToken)
            : await (await GetQueryableAsync()).Where(predicate).ToListAsync(cancellationToken);
    }

    public async override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).LongCountAsync(cancellationToken);
    }

    public async override Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails
            ? await WithDetailsAsync()
            : await GetQueryableAsync();

        return await queryable
            .OrderByIf<TEntity, IQueryable<TEntity>>(!sorting.IsNullOrWhiteSpace(), sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        return (await GetDbSetAsync()).AsQueryable().AsNoTrackingIf(!ShouldTrackingEntityChange());
    }

    public async override Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(cancellationToken)
            : await (await GetQueryableAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var dbSet = context.Set<TEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, cancellationToken);
    }

    public async override Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var dbSet = context.Set<TEntity>();
        await dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default)
        where TProperty : class
    {
        await (await GetDbContextAsync())
            .Entry(entity)
            .Collection(propertyExpression)
            .LoadAsync(cancellationToken);
    }

    public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty?>> propertyExpression,
        CancellationToken cancellationToken = default)
        where TProperty : class
    {
        await (await GetDbContextAsync())
            .Entry(entity)
            .Reference(propertyExpression)
            .LoadAsync(cancellationToken);
    }

    public async override Task<IQueryable<TEntity>> WithDetailsAsync()
    {
        return await GetQueryableAsync();
    }

    public async override Task<IQueryable<TEntity>> WithDetailsAsync(
        params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails(
            await GetQueryableAsync(),
            propertySelectors
        );
    }

    private static IQueryable<TEntity> IncludeDetails(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>[] propertySelectors)
    {
        if (!propertySelectors.IsNullOrEmpty())
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }

    protected virtual void CheckAndSetId(TEntity entity)
    {
        if (entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entityWithGuidId);
        }
    }

    protected virtual void TrySetGuidId(IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        EntityHelper.TrySetId(
            entity,
            () => GuidGenerator.Create(),
            true
        );
    }
}

public class EfCoreRepository<TDbContext, TEntity, TKey>(
    TDbContext dbContext,
    IServiceProvider serviceProvider)
    : EfCoreRepository<TDbContext, TEntity>(dbContext, serviceProvider),
        IEfCoreRepository<TEntity, TKey>
    where TDbContext : class, IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public virtual async Task<TEntity?> FindAsync(TKey id, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).OrderBy(e => e.Id)
                .FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken)
            : !ShouldTrackingEntityChange()
                ? await (await GetQueryableAsync()).OrderBy(e => e.Id)
                    .FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken)
                : await (await GetDbSetAsync()).FindAsync(new object[] { id! }, cancellationToken);
    }

    public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, cancellationToken);
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        var entities = await (await GetDbSetAsync()).Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, cancellationToken);
    }
}