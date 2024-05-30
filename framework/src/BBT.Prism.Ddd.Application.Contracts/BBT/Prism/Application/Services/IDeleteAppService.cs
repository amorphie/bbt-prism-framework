using System.Threading.Tasks;

namespace BBT.Prism.Application.Services;

public interface IDeleteAppService<in TKey> : IApplicationService
{
    Task DeleteAsync(TKey id);
}
