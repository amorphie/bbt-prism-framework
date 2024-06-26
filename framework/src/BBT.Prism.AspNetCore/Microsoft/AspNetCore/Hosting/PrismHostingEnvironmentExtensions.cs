using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting;

public static class PrismHostingEnvironmentExtensions
{
    public static IConfigurationRoot BuildConfiguration(
        this IWebHostEnvironment env,
        PrismConfigurationBuilderOptions? options = null)
    {
        options ??= new PrismConfigurationBuilderOptions();

        if (options.BasePath.IsNullOrEmpty())
        {
            options.BasePath = env.ContentRootPath;
        }

        if (options.EnvironmentName.IsNullOrEmpty())
        {
            options.EnvironmentName = env.EnvironmentName;
        }

        return ConfigurationHelper.BuildConfiguration(options);
    }
}