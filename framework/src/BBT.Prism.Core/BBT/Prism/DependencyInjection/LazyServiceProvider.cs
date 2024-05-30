using System;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.DependencyInjection;

public sealed class LazyServiceProvider(IServiceProvider serviceProvider) :
    CachedServiceProviderBase(serviceProvider),
    ILazyServiceProvider
{
    public T LazyGetRequiredService<T>()
    {
        return (T)LazyGetRequiredService(typeof(T));
    }

    public object LazyGetRequiredService(Type serviceType)
    {
        return this.GetRequiredService(serviceType);
    }

    public T? LazyGetService<T>()
    {
        return (T?)LazyGetService(typeof(T));
    }

    public object? LazyGetService(Type serviceType)
    {
        return GetService(serviceType);
    }

    public T LazyGetService<T>(T defaultValue)
    {
        return GetService(defaultValue);
    }

    public object LazyGetService(Type serviceType, object defaultValue)
    {
        return GetService(serviceType, defaultValue);
    }

    public T LazyGetService<T>(Func<IServiceProvider, object> factory)
    {
        return GetService<T>(factory);
    }

    public object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
    {
        return GetService(serviceType, factory);
    }
}
