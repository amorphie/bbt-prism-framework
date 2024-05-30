using BBT.Prism;
using BBT.Prism.Application.Dtos;

namespace System.Linq;

public static class PrismPagingQueryableExtensions
{
    /// <summary>
    /// Used for paging with an <see cref="IPagedResultRequest"/> object.
    /// </summary>
    /// <param name="query">Queryable to apply paging</param>
    /// <param name="pagedResultRequest">An object implements <see cref="IPagedResultRequest"/> interface</param>
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
    {
        Check.NotNull(query, nameof(query));

        return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
    }
}