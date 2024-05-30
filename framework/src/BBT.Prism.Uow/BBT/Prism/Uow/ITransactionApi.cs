using System;
using System.Threading;
using System.Threading.Tasks;

namespace BBT.Prism.Uow;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}