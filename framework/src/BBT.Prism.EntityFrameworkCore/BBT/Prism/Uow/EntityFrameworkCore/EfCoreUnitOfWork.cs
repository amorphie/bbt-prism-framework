using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BBT.Prism.Uow.EntityFrameworkCore;

public class EfCoreUnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork, ISupportsSavingChanges
    where TDbContext : DbContext, IEfCoreDbContext
{
    public Guid Id => Guid.NewGuid();

    public async Task<ITransactionApi> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfCoreTransactionApi(transaction);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}