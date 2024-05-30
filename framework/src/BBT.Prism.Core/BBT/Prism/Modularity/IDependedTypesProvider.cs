using System;
using JetBrains.Annotations;

namespace BBT.Prism.Modularity;

public interface IDependedTypesProvider
{
    [NotNull]
    Type[] GetDependedTypes();
}