using Microsoft.Extensions.Options;

namespace BBT.Prism.Dapr;

public class DaprApiTokenProvider(IOptions<PrismDaprOptions> options) : IDaprApiTokenProvider
{
    protected PrismDaprOptions Options { get; } = options.Value;

    public virtual string? GetDaprApiToken()
    {
        return Options.DaprApiToken;
    }

    public virtual string? GetAppApiToken()
    {
        return Options.AppApiToken;
    }
}