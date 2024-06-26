using System;

namespace BBT.Prism.Application.Dtos;

/// <summary>
/// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
/// </summary>
[Serializable]
public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
{
    public virtual string? Sorting { get; set; }
}