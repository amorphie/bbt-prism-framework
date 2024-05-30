using System;
using Microsoft.Extensions.DependencyInjection;
using BBT.Prism.Modularity;

namespace BBT.Prism.Testing;

public abstract class PrismIntegratedTest<TStartupModule> : PrismTestBaseWithServiceProvider, IDisposable
    where TStartupModule : IPrismModule
{
    protected IPrismApplication Application { get; }

    protected IServiceProvider RootServiceProvider { get; }

    protected IServiceScope TestServiceScope { get; }

    protected PrismIntegratedTest()
    {
        var services = CreateServiceCollection();

        BeforeAddApplication(services);

        var application = services.AddApplication<TStartupModule>(SetPrismApplicationCreationOptions);
        Application = application;

        AfterAddApplication(services);

        RootServiceProvider = CreateServiceProvider(services);
        TestServiceScope = RootServiceProvider.CreateScope();

        application.Initialize(TestServiceScope.ServiceProvider);
        ServiceProvider = Application.ServiceProvider;
    }

    protected virtual IServiceCollection CreateServiceCollection()
    {
        return new ServiceCollection();
    }

    protected virtual void BeforeAddApplication(IServiceCollection services)
    {
    }

    protected virtual void SetPrismApplicationCreationOptions(PrismApplicationCreationOptions options)
    {
    }

    protected virtual void AfterAddApplication(IServiceCollection services)
    {
    }

    protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProviderFromFactory();
    }

    public virtual void Dispose()
    {
        Application.Shutdown();
        TestServiceScope.Dispose();
        Application.Dispose();
    }
}