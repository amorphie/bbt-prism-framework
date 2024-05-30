using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Domain.Entities;

namespace BBT.Prism.Domain.Repositories;

public static class RepositoryAsyncExtensions
{
    #region Contains

    public async static Task<bool> ContainsAsync<T>(
        this IReadOnlyRepository<T> repository,
        T item,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.ContainsAsync(queryable, item, cancellationToken);
    }

    #endregion

    #region Any/All

    public async static Task<bool> AnyAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AnyAsync(queryable, cancellationToken);
    }

    public async static Task<bool> AnyAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AnyAsync(queryable, predicate, cancellationToken);
    }

    public async static Task<bool> AllAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AllAsync(queryable, predicate, cancellationToken);
    }

    #endregion

    #region Count/LongCount

    public async static Task<int> CountAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.CountAsync(queryable, cancellationToken);
    }

    public async static Task<int> CountAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.CountAsync(queryable, predicate, cancellationToken);
    }

    public async static Task<long> LongCountAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.LongCountAsync(queryable, cancellationToken);
    }

    public async static Task<long> LongCountAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.LongCountAsync(queryable, predicate, cancellationToken);
    }

    #endregion

    #region First/FirstOrDefault

    public async static Task<T> FirstAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.FirstAsync(queryable, cancellationToken);
    }

    public async static Task<T> FirstAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.FirstAsync(queryable, predicate, cancellationToken);
    }

    public async static Task<T?> FirstOrDefaultAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.FirstOrDefaultAsync(queryable, cancellationToken);
    }

    public async static Task<T?> FirstOrDefaultAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.FirstOrDefaultAsync(queryable, predicate, cancellationToken);
    }

    #endregion

    #region Last/LastOrDefault

    public async static Task<T> LastAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.LastAsync(queryable, cancellationToken);
    }

    public async static Task<T> LastAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.LastAsync(queryable, predicate, cancellationToken);
    }

    public async static Task<T?> LastOrDefaultAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.LastOrDefaultAsync(queryable, cancellationToken);
    }

    public async static Task<T?> LastOrDefaultAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.LastOrDefaultAsync(queryable, predicate, cancellationToken);
    }

    #endregion

    #region Single/SingleOrDefault

    public async static Task<T> SingleAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SingleAsync(queryable, cancellationToken);
    }

    public async static Task<T> SingleAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SingleAsync(queryable, predicate, cancellationToken);
    }

    public async static Task<T?> SingleOrDefaultAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SingleOrDefaultAsync(queryable, cancellationToken);
    }

    public async static Task<T?> SingleOrDefaultAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SingleOrDefaultAsync(queryable, predicate, cancellationToken);
    }

    #endregion

    #region Min

    public async static Task<T> MinAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.MinAsync(queryable, cancellationToken);
    }

    public async static Task<TResult> MinAsync<T, TResult>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.MinAsync(queryable, selector, cancellationToken);
    }

    #endregion

    #region Max

    public async static Task<T> MaxAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.MaxAsync(queryable, cancellationToken);
    }

    public async static Task<TResult> MaxAsync<T, TResult>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.MaxAsync(queryable, selector, cancellationToken);
    }

    #endregion

    #region Sum

    public async static Task<decimal> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, decimal>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<decimal?> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, decimal?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<int> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, int>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<int?> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, int?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<long> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, long>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<long?> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, long?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, double>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double?> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, double?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<float> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, float>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    public async static Task<float?> SumAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, float?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
    }

    #endregion

    #region Average

    public async static Task<decimal> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, decimal>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<decimal?> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, decimal?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, int>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double?> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, int?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, long>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double?> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, long?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, double>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<double?> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, double?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    public async static Task<float?> AverageAsync<T>(
        this IReadOnlyRepository<T> repository,
        Expression<Func<T, float?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
    }

    #endregion

    #region ToList/Array

    public async static Task<List<T>> ToListAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.ToListAsync(queryable, cancellationToken);
    }

    public async static Task<T[]> ToArrayAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        var queryable = await repository.GetQueryableAsync();
        return await repository.AsyncExecuter.ToArrayAsync(queryable, cancellationToken);
    }

    #endregion
}