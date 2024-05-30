using System.Threading;
using System.Threading.Tasks;

namespace BBT.Prism.Uow;

public interface ISupportsSavingChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}