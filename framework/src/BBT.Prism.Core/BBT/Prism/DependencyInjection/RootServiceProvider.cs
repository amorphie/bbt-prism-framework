using System;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.DependencyInjection;

public class RootServiceProvider(IObjectAccessor<IServiceProvider> objectAccessor) : IRootServiceProvider
{
    protected IServiceProvider ServiceProvider { get; } = objectAccessor.Value!;

    public virtual object? GetService(Type serviceType)
    {
        return ServiceProvider.GetService(serviceType);
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        return ServiceProvider.GetKeyedService(serviceType, serviceKey);
    }

    public virtual object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        return ServiceProvider.GetRequiredKeyedService(serviceType, serviceKey);
    }
}