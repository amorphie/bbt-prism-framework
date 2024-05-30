using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Testing;

public class PrismAsyncIntegratedTest<TStartupModule> : PrismTestBaseWithServiceProvider
    where TStartupModule : IPrismModule
{
    protected IPrismApplication Application { get; set; } = default!;

    protected IServiceProvider RootServiceProvider { get; set; } = default!;

    protected IServiceScope TestServiceScope { get; set; } = default!;

    public virtual async Task InitializeAsync()
    {
        var services = await CreateServiceCollectionAsync();

        await BeforeAddApplicationAsync(services);
        var application = await services.AddApplicationAsync<TStartupModule>(await SetPrismApplicationCreationOptionsAsync());
        await AfterAddApplicationAsync(services);

        RootServiceProvider = await CreateServiceProviderAsync(services);
        TestServiceScope = RootServiceProvider.CreateScope();
        await application.InitializeAsync(TestServiceScope.ServiceProvider);
        ServiceProvider = application.ServiceProvider;
        Application = application;

        await InitializeServicesAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await Application.ShutdownAsync();
        TestServiceScope.Dispose();
        Application.Dispose();
    }

    protected virtual Task<IServiceCollection> CreateServiceCollectionAsync()
    {
        return Task.FromResult<IServiceCollection>(new ServiceCollection());
    }

    protected virtual Task BeforeAddApplicationAsync(IServiceCollection services)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<Action<PrismApplicationCreationOptions>> SetPrismApplicationCreationOptionsAsync()
    {
        return Task.FromResult<Action<PrismApplicationCreationOptions>>(_ => { });
    }

    protected virtual Task AfterAddApplicationAsync(IServiceCollection services)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<IServiceProvider> CreateServiceProviderAsync(IServiceCollection services)
    {
        return Task.FromResult(services.BuildServiceProviderFromFactory());
    }

    protected virtual Task InitializeServicesAsync()
    {
        return Task.CompletedTask;
    }
}