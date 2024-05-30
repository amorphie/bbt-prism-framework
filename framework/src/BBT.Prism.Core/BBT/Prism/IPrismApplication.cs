using System;
using System.Threading.Tasks;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism;

public interface IPrismApplication : IModuleContainer, IApplicationInfoAccessor, IDisposable
{
    Type StartupModuleType { get; }
    IServiceCollection Services { get; }
    IServiceProvider ServiceProvider { get; }
    Task ConfigureServicesAsync();
    Task ShutdownAsync();
    void Shutdown();
}