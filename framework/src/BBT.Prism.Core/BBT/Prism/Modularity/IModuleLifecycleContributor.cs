using System.Threading.Tasks;

namespace BBT.Prism.Modularity;

public interface IModuleLifecycleContributor
{
    Task InitializeAsync(ApplicationInitializationContext context, IPrismModule module);

    void Initialize(ApplicationInitializationContext context, IPrismModule module);

    Task ShutdownAsync(ApplicationShutdownContext context, IPrismModule module);

    void Shutdown(ApplicationShutdownContext context, IPrismModule module);
}