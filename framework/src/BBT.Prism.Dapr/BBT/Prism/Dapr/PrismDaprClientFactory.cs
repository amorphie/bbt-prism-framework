using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BBT.Prism.Tracing;
using Dapr.Client;
using Microsoft.Extensions.Options;

namespace BBT.Prism.Dapr;

public sealed class PrismDaprClientFactory : IPrismDaprClientFactory
{
    private PrismDaprOptions DaprOptions { get; }
    private JsonSerializerOptions JsonSerializerOptions { get; }
    private IDaprApiTokenProvider DaprApiTokenProvider { get; }
    private ICorrelationIdProvider CorrelationIdProvider { get; }
    private IOptions<CorrelationIdOptions> CorrelationIdOptions { get; }

    public PrismDaprClientFactory(
        IOptions<PrismDaprOptions> options,
        IDaprApiTokenProvider daprApiTokenProvider,
        ICorrelationIdProvider correlationIdProvider,
        IOptions<CorrelationIdOptions> correlationIdOptions)
    {
        DaprApiTokenProvider = daprApiTokenProvider;
        CorrelationIdProvider = correlationIdProvider;
        CorrelationIdOptions = correlationIdOptions;
        DaprOptions = options.Value;
        JsonSerializerOptions = CreateJsonSerializerOptions();
    }

    public Task<DaprClient> CreateAsync(Action<DaprClientBuilder>? builderAction = null)
    {
        var builder = new DaprClientBuilder()
            .UseJsonSerializationOptions(JsonSerializerOptions);

        if (!DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseHttpEndpoint(DaprOptions.HttpEndpoint);
        }

        if (!DaprOptions.GrpcEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseGrpcEndpoint(DaprOptions.GrpcEndpoint);
        }

        var apiToken = DaprApiTokenProvider.GetDaprApiToken();
        if (!apiToken.IsNullOrWhiteSpace())
        {
            builder.UseDaprApiToken(apiToken);
        }

        builderAction?.Invoke(builder);

        return Task.FromResult(builder.Build());
    }

    public Task<HttpClient> CreateHttpClientAsync(
        string? appId = null,
        string? daprEndpoint = null,
        string? daprApiToken = null)
    {
        if (daprEndpoint.IsNullOrWhiteSpace() &&
            !DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
        {
            daprEndpoint = DaprOptions.HttpEndpoint;
        }

        var httpClient = DaprClient.CreateInvokeHttpClient(
            appId,
            daprEndpoint,
            daprApiToken ?? DaprApiTokenProvider.GetDaprApiToken()
        );

        AddHeaders(httpClient);

        return Task.FromResult(httpClient);
    }

    private void AddHeaders(HttpClient httpClient)
    {
        //CorrelationId
        httpClient.DefaultRequestHeaders.Add(CorrelationIdOptions.Value.HttpHeaderName, CorrelationIdProvider.Get());

        //Culture
        var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if (!currentCulture.IsNullOrEmpty())
        {
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCulture));
        }

        //X-Requested-With
        httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    }

    private JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return new JsonSerializerOptions(new DaprJsonSerializerOptions().JsonSerializerOptions);
    }
}