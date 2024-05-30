using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism;

public class PrismApplicationCreationOptions([NotNull] IServiceCollection services)
{
    [NotNull]
    public IServiceCollection Services { get; } = Check.NotNull(services, nameof(services));

    [NotNull]
    public PrismConfigurationBuilderOptions Configuration { get; } = new();

    public bool SkipConfigureServices { get; set; }

    public string? ApplicationName { get; set; }

    public string? Environment { get; set; }
}