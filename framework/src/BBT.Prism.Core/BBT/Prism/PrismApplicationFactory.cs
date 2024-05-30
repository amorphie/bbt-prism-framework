using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism;

public static class PrismApplicationFactory
{
    public async static Task<IApplicationServiceProvider> CreateAsync<TStartupModule>(
        IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IPrismModule
    {
        var app = Create(typeof(TStartupModule), services, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IApplicationServiceProvider> CreateAsync(
        Type startupModuleType,
        IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
    {
        var app = new ApplicationServiceProvider(startupModuleType, services, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }
    
    public static IApplicationServiceProvider Create<TStartupModule>(
        IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IPrismModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    public static IApplicationServiceProvider Create(
        Type startupModuleType,
        IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
    {
        return new ApplicationServiceProvider(startupModuleType, services, optionsAction);
    }
}
