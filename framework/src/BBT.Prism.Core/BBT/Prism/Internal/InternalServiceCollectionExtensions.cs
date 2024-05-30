using System.Linq;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Guids;
using BBT.Prism.Logging;
using BBT.Prism.Modularity;
using BBT.Prism.Reflection;
using BBT.Prism.Timing;
using BBT.Prism.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BBT.Prism.Internal;

static internal class InternalServiceCollectionExtensions
{
    static internal void AddCoreServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddLogging();
        services.AddLocalization();
    }

    static internal void AddCorePrismServices(
        this IServiceCollection services,
        IPrismApplication prismApplication,
        PrismApplicationCreationOptions applicationCreationOptions)
    {
        var moduleLoader = new ModuleLoader();
        var assemblyFinder = new AssemblyFinder(prismApplication);
        var typeFinder = new TypeFinder(assemblyFinder);

        if (!services.IsAdded<IConfiguration>())
        {
            services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(
                    applicationCreationOptions.Configuration
                )
            );
        }

        services.AddSingleton<ICorrelationIdProvider, DefaultCorrelationIdProvider>();
        services.AddSingleton<IGuidGenerator>(SimpleGuidGenerator.Instance);
        services.AddTransient<IClock, Clock>();

        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();
        services.AddScoped<IRootServiceProvider, RootServiceProvider>();
        services.TryAddSingleton<IModuleManager, ModuleManager>();
        services.TryAddSingleton<IModuleLoader>(moduleLoader);
        services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
        services.TryAddSingleton<ITypeFinder>(typeFinder);
        services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());
        
        services.Configure<PrismModuleLifecycleOptions>(options =>
        {
            options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
        });

        var contributors = typeof(IModuleLifecycleContributor).Assembly.GetTypes()
            .Where(t => typeof(IModuleLifecycleContributor).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .ToList();
        
        foreach (var contributor in contributors)
        {
            services.AddTransient(contributor);
        }
    }
}