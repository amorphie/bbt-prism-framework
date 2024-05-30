using System.Threading.Tasks;

namespace BBT.Prism.Uow;

public interface IDbContextProvider<TDbContext>
    where TDbContext : class
{
    Task<TDbContext> GetDbContextAsync();
}