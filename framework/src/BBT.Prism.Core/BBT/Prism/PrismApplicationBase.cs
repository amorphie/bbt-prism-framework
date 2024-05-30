using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Internal;
using BBT.Prism.Logging;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BBT.Prism;

public abstract class PrismApplicationBase : IPrismApplication
{
    public Type StartupModuleType { get; }
    public IServiceProvider ServiceProvider { get; private set; } = default!;
    public IServiceCollection Services { get; }
    public IReadOnlyList<IPrismModuleDescriptor> Modules { get; }
    public string? ApplicationName { get; }
    public string InstanceId { get; } = Guid.NewGuid().ToString();

    private bool _configuredServices;
    
    internal PrismApplicationBase(
        Type startupModuleType,
        IServiceCollection services,
        Action<PrismApplicationCreationOptions>? optionsAction)
    {
        Check.NotNull(startupModuleType, nameof(startupModuleType));
        Check.NotNull(services, nameof(services));

        StartupModuleType = startupModuleType;
        Services = services;

        services.TryAddObjectAccessor<IServiceProvider>();

        var options = new PrismApplicationCreationOptions(services);
        optionsAction?.Invoke(options);

        ApplicationName = GetApplicationName(options);

        services.AddSingleton<IPrismApplication>(this);
        services.AddSingleton<IApplicationInfoAccessor>(this);
        services.AddSingleton<IModuleContainer>(this);
        services.AddSingleton<IPrismHostEnvironment>(new PrismHostEnvironment()
        {
            EnvironmentName = options.Environment
        });

        services.AddCoreServices();
        services.AddCorePrismServices(this, options);

        Modules = LoadModules(services, options);

        if (!options.SkipConfigureServices)
        {
            ConfigureServices();
        }
    }
    
    public virtual async Task ShutdownAsync()
    {
        using var scope = ServiceProvider.CreateScope();
        await scope.ServiceProvider
            .GetRequiredService<IModuleManager>()
            .ShutdownModulesAsync(new ApplicationShutdownContext(scope.ServiceProvider));
    }

    public virtual void Shutdown()
    {
        using var scope = ServiceProvider.CreateScope();
        scope.ServiceProvider
            .GetRequiredService<IModuleManager>()
            .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
    }

    public virtual void Dispose()
    {
        //No-op
    }

    protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
    }

    protected virtual async Task InitializeModulesAsync()
    {
        using var scope = ServiceProvider.CreateScope();
        WriteInitLogs(scope.ServiceProvider);
        await scope.ServiceProvider
            .GetRequiredService<IModuleManager>()
            .InitializeModulesAsync(new ApplicationInitializationContext(scope.ServiceProvider));
    }

    protected virtual void InitializeModules()
    {
        using var scope = ServiceProvider.CreateScope();
        WriteInitLogs(scope.ServiceProvider);
        scope.ServiceProvider
            .GetRequiredService<IModuleManager>()
            .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
    }

    protected virtual void WriteInitLogs(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger<PrismApplicationBase>>();
        if (logger == null)
        {
            return;
        }

        var initLogger = serviceProvider.GetRequiredService<IInitLoggerFactory>().Create<PrismApplicationBase>();

        foreach (var entry in initLogger.Entries)
        {
            logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
        }

        initLogger.Entries.Clear();
    }

    protected virtual IReadOnlyList<IPrismModuleDescriptor> LoadModules(IServiceCollection services, PrismApplicationCreationOptions options)
    {
        return services
            .GetSingletonInstance<IModuleLoader>()
            .LoadModules(
                services,
                StartupModuleType
            );
    }
    
    public virtual async Task ConfigureServicesAsync()
    {
        CheckMultipleConfigureServices();

        var context = new ModuleConfigurationContext(Services);
        Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is PrismModule prismModule)
            {
                prismModule.ModuleConfigurationContext = context;
            }
        }
        
        //PreConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
        {
            try
            {
                await ((IPreConfigureServices)module.Instance).PreConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new PrismInitializationException($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        var assemblies = new HashSet<Assembly>();

        //ConfigureServices
        foreach (var module in Modules)
        {
            if (module.Instance is PrismModule { SkipAutoServiceRegistration: false })
            {
                foreach (var assembly in module.AllAssemblies)
                {
                    if (!assemblies.Contains(assembly))
                    {
                        // Services.AddAssembly(assembly);
                        assemblies.Add(assembly);
                    }
                }
            }

            try
            {
                await module.Instance.ConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new PrismInitializationException($"An error occurred during {nameof(IPrismModule.ConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        foreach (var module in Modules)
        {
            if (module.Instance is PrismModule prismModule)
            {
                prismModule.ModuleConfigurationContext = null!;
            }
        }
        
        ExecuteRegistrationAction(context.Services);

        _configuredServices = true;

        TryToSetEnvironment(Services);
    }

    public virtual void ConfigureServices()
    {
        CheckMultipleConfigureServices();

        var context = new ModuleConfigurationContext(Services);
        Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is PrismModule prismModule)
            {
                prismModule.ModuleConfigurationContext = context;
            }
        }
        
        //PreConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
        {
            try
            {
                ((IPreConfigureServices)module.Instance).PreConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new PrismInitializationException($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }


        var assemblies = new HashSet<Assembly>();

        //ConfigureServices
        foreach (var module in Modules)
        {
            if (module.Instance is PrismModule { SkipAutoServiceRegistration: false })
            {
                foreach (var assembly in module.AllAssemblies)
                {
                    if (!assemblies.Contains(assembly))
                    {
                        // Services.AddAssembly(assembly);
                        assemblies.Add(assembly);
                    }
                }
            }

            try
            {
                module.Instance.ConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new PrismInitializationException($"An error occurred during {nameof(IPrismModule.ConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }
        
        foreach (var module in Modules)
        {
            if (module.Instance is PrismModule prismModule)
            {
                prismModule.ModuleConfigurationContext = null!;
            }
        }
        
        ExecuteRegistrationAction(context.Services);

        _configuredServices = true;

        TryToSetEnvironment(Services);
    }
    
    private void CheckMultipleConfigureServices()
    {
        if (_configuredServices)
        {
            throw new PrismInitializationException("Services have already been configured! If you call ConfigureServicesAsync method, you must have set PrismApplicationCreationOptions.SkipConfigureServices to true before.");
        }
    }
    
    private void ExecuteRegistrationAction(IServiceCollection services)
    {
        // Register callback actions
        var registrationActions = services.GetRegistrationActionList();
        foreach (var serviceDescriptor in services)
        {
            if (serviceDescriptor.ImplementationType != null)
            {
                var registeredContext = new OnServiceRegisteredContext(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType);
                foreach (var action in registrationActions)
                {
                    action(registeredContext);
                }
            }
        }
    }

    private static string? GetApplicationName(PrismApplicationCreationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ApplicationName))
        {
            return options.ApplicationName!;
        }

        var configuration = options.Services.GetConfigurationOrNull();
        if (configuration != null)
        {
            var appNameConfig = configuration["ApplicationName"];
            if (!string.IsNullOrWhiteSpace(appNameConfig))
            {
                return appNameConfig!;
            }
        }

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            return entryAssembly.GetName().Name;
        }

        return null;
    }

    private static void TryToSetEnvironment(IServiceCollection services)
    {
        var prismHostEnvironment = services.GetSingletonInstance<IPrismHostEnvironment>();
        if (prismHostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
        {
            prismHostEnvironment.EnvironmentName = Environments.Production;
        }
    }
    
}