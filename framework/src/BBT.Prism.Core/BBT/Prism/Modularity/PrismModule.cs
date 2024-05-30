using System;
using System.Reflection;
using System.Threading.Tasks;
using BBT.Prism.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Modularity;

public abstract class PrismModule : 
    IPrismModule,
    IOnApplicationInitialization,
    IOnPostApplicationInitialization,
    IOnApplicationShutdown,
    IPreConfigureServices
{
    protected internal bool SkipAutoServiceRegistration { get; protected set; }

    protected internal ModuleConfigurationContext ModuleConfigurationContext {
        get {
            if (_serviceConfigurationContext == null)
            {
                throw new PrismException($"{nameof(ModuleConfigurationContext)} is only available in the {nameof(ConfigureServices)} methods.");
            }

            return _serviceConfigurationContext;
        }
        internal set => _serviceConfigurationContext = value;
    }

    private ModuleConfigurationContext? _serviceConfigurationContext;

    public virtual Task PreConfigureServicesAsync(ModuleConfigurationContext context)
    {
        PreConfigureServices(context);
        return Task.CompletedTask;
    }

    public virtual void PreConfigureServices(ModuleConfigurationContext context)
    {

    }
    
    public virtual Task ConfigureServicesAsync(ModuleConfigurationContext context)
    {
        ConfigureServices(context);
        return Task.CompletedTask;
    }

    public virtual void ConfigureServices(ModuleConfigurationContext context)
    {
       
    }
    
    public virtual Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnApplicationInitialization(context);
        return Task.CompletedTask;
    }

    public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
    
    public virtual Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnPostApplicationInitialization(context);
        return Task.CompletedTask;
    }

    public virtual void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {

    }
    
    public virtual Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        OnApplicationShutdown(context);
        return Task.CompletedTask;
    }

    public virtual void OnApplicationShutdown(ApplicationShutdownContext context)
    {

    }
    
    public static bool IsPrismModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return
            typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IPrismModule).GetTypeInfo().IsAssignableFrom(type);
    }

    static internal void CheckPrismModuleType(Type moduleType)
    {
        if (!IsPrismModule(moduleType))
        {
            throw new ArgumentException("Given type is not an PRISM module: " + moduleType.AssemblyQualifiedName);
        }
    }
    
    protected void PreConfigure<TOptions>(Action<TOptions> configureOptions)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.PreConfigure(configureOptions);
    }

    protected void Configure<TOptions>(Action<TOptions> configureOptions)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.Configure(configureOptions);
    }

    protected void Configure<TOptions>(string name, Action<TOptions> configureOptions)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.Configure(name, configureOptions);
    }

    protected void Configure<TOptions>(IConfiguration configuration)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.Configure<TOptions>(configuration);
    }

    protected void Configure<TOptions>(IConfiguration configuration, Action<BinderOptions> configureBinder)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.Configure<TOptions>(configuration, configureBinder);
    }

    protected void Configure<TOptions>(string name, IConfiguration configuration)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.Configure<TOptions>(name, configuration);
    }

    protected void PostConfigure<TOptions>(Action<TOptions> configureOptions)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.PostConfigure(configureOptions);
    }

    protected void PostConfigureAll<TOptions>(Action<TOptions> configureOptions)
        where TOptions : class
    {
        ModuleConfigurationContext.Services.PostConfigureAll(configureOptions);
    }
}