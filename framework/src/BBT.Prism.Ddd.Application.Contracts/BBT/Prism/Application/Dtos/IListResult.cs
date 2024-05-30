using System.Collections.Generic;

namespace BBT.Prism.Application.Dtos;

public interface IListResult<T>
{
    /// <summary>
    /// List of items.
    /// </summary>
    IReadOnlyList<T> Items { get; set; }
}