using System;
using BBT.Prism.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Dapr;

public class PrismDaprModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureDaprOptions(configuration);
        context.Services.AddSingleton<IPrismDaprClientFactory, PrismDaprClientFactory>();
        context.Services.AddSingleton<IDaprApiTokenProvider, DaprApiTokenProvider>();
        context.Services.AddTransient<IDaprSerializer, Utf8JsonDaprSerializer>();
    }
    
    private void ConfigureDaprOptions(IConfiguration configuration)
    {
        Configure<PrismDaprOptions>(configuration.GetSection("Dapr"));
        Configure<PrismDaprOptions>(options =>
        {
            if (options.DaprApiToken.IsNullOrWhiteSpace())
            {
                var confEnv = configuration["DAPR_API_TOKEN"];
                if (!confEnv.IsNullOrWhiteSpace())
                {
                    options.DaprApiToken = confEnv!;
                }
                else
                {
                    var env = Environment.GetEnvironmentVariable("DAPR_API_TOKEN");
                    if (!env.IsNullOrWhiteSpace())
                    {
                        options.DaprApiToken = env!;
                    }
                }
            }

            // ReSharper disable once InvertIf
            if (options.AppApiToken.IsNullOrWhiteSpace())
            {
                var confEnv = configuration["APP_API_TOKEN"];
                if (!confEnv.IsNullOrWhiteSpace())
                {
                    options.AppApiToken = confEnv!;
                }
                else
                {
                    var env = Environment.GetEnvironmentVariable("APP_API_TOKEN");
                    if (!env.IsNullOrWhiteSpace())
                    {
                        options.AppApiToken = env!;
                    }
                }
            }
        });
    }
}