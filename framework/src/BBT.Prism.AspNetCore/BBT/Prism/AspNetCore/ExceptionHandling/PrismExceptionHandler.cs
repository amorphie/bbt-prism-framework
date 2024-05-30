using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace BBT.Prism.AspNetCore.ExceptionHandling;

public class PrismExceptionHandler : IExceptionHandler
{
    internal const string ErrorFormat = "_BBTErrorFormat";
    private readonly Func<object, Task> _clearCacheHeadersDelegate;
    private readonly ILogger<PrismExceptionHandler> _logger;

    public PrismExceptionHandler(ILogger<PrismExceptionHandler> logger)
    {
        _logger = logger;
        _clearCacheHeadersDelegate = ClearCacheHeaders;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        await HandleAndWrapException(httpContext, exception, cancellationToken);

        return true;
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogException(exception);
        var errorInfoConverter = httpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
        var statusCodeFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
        var exceptionHandlingOptions = httpContext.RequestServices
            .GetRequiredService<IOptions<PrismExceptionHandlingOptions>>().Value;

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = (int)statusCodeFinder.GetStatusCode(httpContext, exception);
        httpContext.Response.OnStarting(_clearCacheHeadersDelegate, httpContext.Response);
        httpContext.Response.Headers.Append(ErrorFormat, "true");
        httpContext.Response.Headers.Append("Content-Type", "application/json");

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(
                new ServiceErrorResponse(
                    errorInfoConverter.Convert(exception, options =>
                    {
                        options.SendExceptionsDetailsToClients =
                            exceptionHandlingOptions.SendExceptionsDetailsToClients;
                        options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
                    })
                )
            ), cancellationToken: cancellationToken);
    }

    private Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    }
}