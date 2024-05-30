using System;
using System.Threading;
using System.Threading.Tasks;

namespace BBT.Prism.Uow;

public interface IUnitOfWork : IDisposable
{
    Guid Id { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<ITransactionApi> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CompleteAsync(CancellationToken cancellationToken = default);
}