using System.Threading.Tasks;

namespace BBT.Prism;

public interface IOnApplicationInitialization
{
    Task OnApplicationInitializationAsync(ApplicationInitializationContext context);

    void OnApplicationInitialization(ApplicationInitializationContext context);
}
