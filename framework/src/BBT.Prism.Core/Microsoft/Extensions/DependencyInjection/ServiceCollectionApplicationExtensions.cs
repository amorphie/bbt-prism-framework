using System;
using System.Threading.Tasks;
using BBT.Prism;
using BBT.Prism.Modularity;
using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionApplicationExtensions
{
    public static IApplicationServiceProvider AddApplication<TStartupModule>(
        this IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IPrismModule
    {
        return PrismApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static IApplicationServiceProvider AddApplication(
        this IServiceCollection services,
        Type startupModuleType,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
    {
        return PrismApplicationFactory.Create(startupModuleType, services, optionsAction);
    }

    public async static Task<IApplicationServiceProvider> AddApplicationAsync<TStartupModule>(
        this IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IPrismModule
    {
        return await PrismApplicationFactory.CreateAsync<TStartupModule>(services,  optionsAction);
    }

    public async static Task<IApplicationServiceProvider> AddApplicationAsync(
        this IServiceCollection services,
        Type startupModuleType,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
    {
        return await PrismApplicationFactory.CreateAsync(startupModuleType, services, optionsAction);
    }

    public static string? GetApplicationName(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfoAccessor>().ApplicationName;
    }

    public static string GetApplicationInstanceId(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfoAccessor>().InstanceId;
    }

    public static IPrismHostEnvironment GetPrismHostEnvironment(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IPrismHostEnvironment>();
    }
}
