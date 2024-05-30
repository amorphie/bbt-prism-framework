using System.Threading.Tasks;

namespace BBT.Prism;

public interface IOnApplicationShutdown
{
    Task OnApplicationShutdownAsync(ApplicationShutdownContext context);

    void OnApplicationShutdown(ApplicationShutdownContext context);
}
