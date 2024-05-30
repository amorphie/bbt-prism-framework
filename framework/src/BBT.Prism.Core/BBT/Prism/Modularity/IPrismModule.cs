using System.Threading.Tasks;

namespace BBT.Prism.Modularity;

public interface IPrismModule
{
    Task ConfigureServicesAsync(ModuleConfigurationContext context);

    void ConfigureServices(ModuleConfigurationContext context);
}