using System.Threading.Tasks;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BBT.Prism.Domain.EntityFrameworkCore;

public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity: class, IEntity
{
    Task<DbContext> GetDbContextAsync();

    Task<DbSet<TEntity>> GetDbSetAsync();
}

public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{

}