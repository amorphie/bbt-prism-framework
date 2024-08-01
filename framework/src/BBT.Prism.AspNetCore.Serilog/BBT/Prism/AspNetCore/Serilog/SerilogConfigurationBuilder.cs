using System;
using System.Collections.Generic;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Async;
using Serilog.Sinks.OpenTelemetry;

namespace BBT.Prism.AspNetCore.Serilog;

public class SerilogConfigurationBuilder(string applicationName, IConfiguration configuration)
{
    private readonly LoggerConfiguration _loggerConfiguration = new();

    public SerilogConfigurationBuilder AddDefaultConfiguration()
    {
        _loggerConfiguration
            .ReadFrom.Configuration(configuration)
#if DEBUG
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("HealthChecks", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithSpan()
            .WriteTo.Async(c => c.File(new CompactJsonFormatter(), $"Logs/{applicationName}-log.json",
                rollingInterval: RollingInterval.Day))
            // .WriteTo.Elasticsearch(new [] { new Uri("http://localhost:9200" )}, opts =>
            // {
            //     opts.DataStream = new DataStreamName("logs", "console-example", "demo");
            //     opts.BootstrapMethod = BootstrapMethod.Failure;
            // })
            #if DEBUG
            .WriteTo.Async(c => c.Console());
            #else
            ;
            #endif
        
        return this;
    }

    public SerilogConfigurationBuilder AddEnrichment(ILogEventEnricher enricher)
    {
        _loggerConfiguration.Enrich.With(enricher);
        return this;
    }
    
    public SerilogConfigurationBuilder AddOpenTelemetry(string? otlpUrl = null)
    {
        var url = otlpUrl ?? configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (url.IsNullOrEmpty())
        {
            throw new InvalidOperationException("OTLP endpoint cannot be empty.");
        }
        
        _loggerConfiguration.WriteTo.OpenTelemetry(cfg =>
        {
            cfg.Endpoint = url!;
            cfg.IncludedData = IncludedData.TraceIdField | IncludedData.SpanIdField;
            AddHeaders(cfg.Headers, configuration["OTEL_EXPORTER_OTLP_HEADERS"]);
            AddResourceAttributes(cfg.ResourceAttributes, configuration["OTEL_RESOURCE_ATTRIBUTES"]);

            void AddHeaders(IDictionary<string, string> headers, string? headerConfig)
            {
                if (!string.IsNullOrEmpty(headerConfig))
                {
                    foreach (var header in headerConfig.Split(','))
                    {
                        var parts = header.Split('=');

                        if (parts.Length == 2)
                        {
                            headers[parts[0]] = parts[1];
                        }
                        else
                        {
                            throw new InvalidOperationException($"Invalid header format: {header}");
                        }
                    }
                }
            }

            void AddResourceAttributes(IDictionary<string, object> attributes, string? attributeConfig)
            {
                if (!string.IsNullOrEmpty(attributeConfig))
                {
                    var parts = attributeConfig.Split('=');

                    if (parts.Length == 2)
                    {
                        attributes[parts[0]] = parts[1];
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid resource attribute format: {attributeConfig}");
                    }
                }
            }
        });
        return this;
    }

    public void Build()
    {
        Log.Logger = _loggerConfiguration.CreateLogger();
    }
}