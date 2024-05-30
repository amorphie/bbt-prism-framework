using System;

namespace BBT.Prism.DependencyInjection;

public interface IClientScopeServiceProviderAccessor
{
    IServiceProvider ServiceProvider { get; }
}
