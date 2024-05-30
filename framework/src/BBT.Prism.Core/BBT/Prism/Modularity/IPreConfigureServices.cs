using System.Threading.Tasks;

namespace BBT.Prism.Modularity;

public interface IPreConfigureServices
{
    Task PreConfigureServicesAsync(ModuleConfigurationContext context);

    void PreConfigureServices(ModuleConfigurationContext context);
}