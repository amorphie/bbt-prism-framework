using System.Threading;
using System.Threading.Tasks;

namespace BBT.Prism.Uow;

public interface ISupportsRollback
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}