using System;
using System.IO;
using BBT.Prism.AspNetCore.Serilog;
using Microsoft.Extensions.Configuration;

namespace BBT.MyProjectName;

public static class SerilogConfigurationHelper
{
    public static void Configure(string applicationName)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var builder = new SerilogConfigurationBuilder(applicationName, configuration)
            .AddDefaultConfiguration();

        // Enable Open Telemetry
        builder.AddOpenTelemetry();
        
        // Example of adding a custom enricher
        // builder.AddEnrichment(new CustomEnricher());

        builder.Build();
    }
}