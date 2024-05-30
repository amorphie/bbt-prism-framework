using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BBT.Prism.Logging;

public interface IInitLogger<out T> : ILogger<T>
{
    public List<PrismInitLogEntry> Entries { get; }
}