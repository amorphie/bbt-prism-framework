using System.Threading.Tasks;

namespace BBT.Prism.Modularity;

public interface IOnPostApplicationInitialization
{
    Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context);

    void OnPostApplicationInitialization(ApplicationInitializationContext context);
}