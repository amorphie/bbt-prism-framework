using System.Threading.Tasks;
using BBT.Prism.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.Prism.Uow.EntityFrameworkCore;

public class EfCoreDbContextProvider<TDbContext>(IDbContextFactory<TDbContext> contextFactory)
    : IDbContextProvider<TDbContext>
    where TDbContext : DbContext, IEfCoreDbContext
{
    public Task<TDbContext> GetDbContextAsync()
    {
        return contextFactory.CreateDbContextAsync();
    }
}
