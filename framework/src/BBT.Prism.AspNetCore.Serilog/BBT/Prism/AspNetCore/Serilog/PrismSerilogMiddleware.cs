using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BBT.Prism.Tracing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace BBT.Prism.AspNetCore.Serilog;

public class PrismSerilogMiddleware(
    IOptions<PrismAspNetCoreSerilogOptions> options,
    ICorrelationIdProvider correlationIdProvider,
    IApplicationInfoAccessor applicationInfoAccessor,
    IPrismHostEnvironment prismHostEnvironment)
    : IMiddleware
{
    private readonly PrismAspNetCoreSerilogOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var enrichers = new List<ILogEventEnricher>();
        AddApplicationInfo(enrichers);
        AddCorrelationId(enrichers);
        AddHeaders(context, enrichers);
        AddPathAndQuery(context, enrichers);
        AddOtherInfo(context, enrichers);
        if (_options.ShouldBodyBeTracked)
        {
            await AddBody(context, enrichers);
        }
        using (LogContext.Push(enrichers.ToArray()))
        {
            await next(context);
        }
    }

    private void AddCorrelationId(List<ILogEventEnricher> enrichers)
    {
        var correlationId = correlationIdProvider.Get();
        if (!string.IsNullOrEmpty(correlationId))
        {
            enrichers.Add(new PropertyEnricher("CorrelationId", correlationId));
        }
    }

    private void AddOtherInfo(HttpContext context, List<ILogEventEnricher> enrichers)
    {
        enrichers.Add(new PropertyEnricher("IpAddress", context.Connection.RemoteIpAddress?.ToString()));
        enrichers.Add(new PropertyEnricher("Host", context.Request.Host.ToString()));
    }

    private void AddApplicationInfo(List<ILogEventEnricher> enrichers)
    {
        enrichers.Add(new PropertyEnricher("Environment", prismHostEnvironment.EnvironmentName));
        enrichers.Add(new PropertyEnricher("ApplicationName", applicationInfoAccessor.ApplicationName));
        enrichers.Add(new PropertyEnricher("ApplicationInstanceId", applicationInfoAccessor.InstanceId));
    }

    private async Task AddBody(HttpContext context, List<ILogEventEnricher> enrichers)
    {
        try
        {
            if (context.Request.ContentLength is > 0)
            {
                context.Request.EnableBuffering();
                string body;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                }

                context.Request.Body.Position = 0;
                enrichers.Add(new PropertyEnricher("Body", body));
            }
        }
        catch
        {
            //No-op
        }
    }

    private void AddHeaders(HttpContext context, List<ILogEventEnricher> enrichers)
    {
        foreach (var header in context.Request.Headers)
        {
            if (_options.Headers.Contains(header.Key.ToLower()))
            {
                enrichers.Add(_options.Wildcards.Contains(header.Key.ToLower())
                    ? new PropertyEnricher($"header.{header.Key}", "*****")
                    : new PropertyEnricher($"header.{header.Key}", header.Value));
            }
        }
    }

    private void AddPathAndQuery(HttpContext context, List<ILogEventEnricher> enrichers)
    {
        if (context.Request.Path.HasValue)
        {
            enrichers.Add(new PropertyEnricher("RequestPath", context.Request.Path.Value));
        }

        foreach (var query in context.Request.Query)
        {
            enrichers.Add(_options.Wildcards.Contains(query.Key.ToLower())
                ? new PropertyEnricher($"query.{query.Key}", "*****")
                : new PropertyEnricher($"query.{query.Key}", query.Value));
        }
    }
}