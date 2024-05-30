using System.Threading.Tasks;

namespace BBT.Prism.Modularity;

public abstract class ModuleLifecycleContributorBase: IModuleLifecycleContributor
{
    public virtual Task InitializeAsync(ApplicationInitializationContext context, IPrismModule module)
    {
        return Task.CompletedTask;
    }

    public virtual void Initialize(ApplicationInitializationContext context, IPrismModule module)
    {
    }

    public virtual Task ShutdownAsync(ApplicationShutdownContext context, IPrismModule module)
    {
        return Task.CompletedTask;
    }

    public virtual void Shutdown(ApplicationShutdownContext context, IPrismModule module)
    {
    }
}
