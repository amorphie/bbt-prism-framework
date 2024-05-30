using System;
using System.Threading.Tasks;
using BBT.Prism;
using BBT.Prism.Modularity;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public async static Task<IApplicationServiceProvider> AddApplicationAsync<TStartupModule>(
        this WebApplicationBuilder builder,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IPrismModule
    {
        return await builder.Services.AddApplicationAsync<TStartupModule>(options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
            optionsAction?.Invoke(options);
            if (options.Environment.IsNullOrWhiteSpace())
            {
                options.Environment = builder.Environment.EnvironmentName;
            }
        });
    }

    public async static Task<IApplicationServiceProvider> AddApplicationAsync(
        this WebApplicationBuilder builder,
        Type startupModuleType,
        Action<PrismApplicationCreationOptions>? optionsAction = null)
    {
        return await builder.Services.AddApplicationAsync(startupModuleType, options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
            optionsAction?.Invoke(options);
            if (options.Environment.IsNullOrWhiteSpace())
            {
                options.Environment = builder.Environment.EnvironmentName;
            }
        });
    }
}