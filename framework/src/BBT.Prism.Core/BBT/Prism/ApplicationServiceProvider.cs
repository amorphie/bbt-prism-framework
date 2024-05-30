using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism;

internal class ApplicationServiceProvider : PrismApplicationBase, IApplicationServiceProvider
{
    public ApplicationServiceProvider(
        Type startupModuleType,
        IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction
    ) : base(
        startupModuleType,
        services,
        optionsAction)
    {
        services.AddSingleton<IApplicationServiceProvider>(this);
    }

    void IApplicationServiceProvider.SetServiceProvider(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new PrismException("Service provider was already set before to another service provider instance.");
            }

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        await InitializeModulesAsync();
    }

    public void Initialize(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}