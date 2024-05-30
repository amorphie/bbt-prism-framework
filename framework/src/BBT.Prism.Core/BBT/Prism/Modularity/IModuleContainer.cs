using System.Collections.Generic;
using JetBrains.Annotations;

namespace BBT.Prism.Modularity;

public interface IModuleContainer
{
    [NotNull]
    IReadOnlyList<IPrismModuleDescriptor> Modules { get; }
}