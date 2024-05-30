using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace BBT.Prism.Uow.EntityFrameworkCore;

public class EfCoreTransactionApi(
    IDbContextTransaction dbContextTransaction)
    : ITransactionApi, ISupportsRollback
{
    public IDbContextTransaction DbContextTransaction { get; } = dbContextTransaction;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await DbContextTransaction.CommitAsync(cancellationToken);
    }

    public void Dispose()
    {
        DbContextTransaction.Dispose();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await DbContextTransaction.RollbackAsync(cancellationToken);
    }
}