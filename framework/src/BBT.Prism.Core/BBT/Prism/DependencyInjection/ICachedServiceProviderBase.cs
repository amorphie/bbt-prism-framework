using System;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.DependencyInjection;

public interface ICachedServiceProviderBase: IKeyedServiceProvider
{
    T GetService<T>(T defaultValue);

    object GetService(Type serviceType, object defaultValue);

    T GetService<T>(Func<IServiceProvider, object> factory);

    object GetService(Type serviceType, Func<IServiceProvider, object> factory);
}
