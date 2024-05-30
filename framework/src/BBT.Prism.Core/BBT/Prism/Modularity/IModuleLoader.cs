using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Modularity;

public interface IModuleLoader
{
    [NotNull]
    IPrismModuleDescriptor[] LoadModules(
        [NotNull] IServiceCollection services,
        [NotNull] Type startupModuleType
    );
}