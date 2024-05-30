using System;
using BBT.Prism.Dapr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.AspNetCore.Dapr;

public sealed class DaprAppApiTokenValidator(IHttpContextAccessor httpContextAccessor) : IDaprAppApiTokenValidator
{
    private IHttpContextAccessor HttpContextAccessor { get; } = httpContextAccessor;
    private HttpContext HttpContext => GetHttpContext();

    public void CheckDaprAppApiToken()
    {
        var expectedAppApiToken = GetConfiguredAppApiTokenOrNull();
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return;
        }

        var headerAppApiToken = GetDaprAppApiTokenOrNull();
        if (headerAppApiToken.IsNullOrWhiteSpace())
        {
            throw new PrismAuthorizationException("Expected Dapr App API Token is not provided! Dapr should set the 'dapr-api-token' HTTP header.");
        }

        if (expectedAppApiToken != headerAppApiToken)
        {
            throw new PrismAuthorizationException("The Dapr App API Token (provided in the 'dapr-api-token' HTTP header) doesn't match the expected value!");
        }
    }

    public bool IsValidDaprAppApiToken()
    {
        var expectedAppApiToken = GetConfiguredAppApiTokenOrNull();
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return true;
        }

        var headerAppApiToken = GetDaprAppApiTokenOrNull();
        return expectedAppApiToken == headerAppApiToken;
    }

    public string? GetDaprAppApiTokenOrNull()
    {
        string? apiTokenHeader = HttpContext.Request.Headers["dapr-api-token"];
        if (string.IsNullOrEmpty(apiTokenHeader) || apiTokenHeader.Length < 1)
        {
            return null;
        }

        return apiTokenHeader;
    }

    private string? GetConfiguredAppApiTokenOrNull()
    {
        return HttpContext
            .RequestServices
            .GetRequiredService<IDaprApiTokenProvider>()
            .GetAppApiToken();
    }

    private HttpContext GetHttpContext()
    {
        return HttpContextAccessor.HttpContext ?? throw new PrismException("HttpContext is not available!");
    }
}
