using System;
using BBT.Prism.EntityFrameworkCore;
using BBT.Prism.EntityFrameworkCore.Interceptors;
using BBT.Prism.Uow;
using BBT.Prism.Uow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class PrismEfCoreServiceCollectionExtensions
{
    public static IServiceCollection AddPrismDbContext<TDbContext>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> options)
        where TDbContext : PrismDbContext<TDbContext>
    {
        services.AddDbContext<TDbContext>((sp, dbContextOptions) =>
        {
            options?.Invoke(dbContextOptions);
            dbContextOptions.AddInterceptors(
                sp.GetRequiredService<AuditInterceptor>()
            );
        });
        services.AddTransient<IUnitOfWork, EfCoreUnitOfWork<TDbContext>>();
        return services;
    }
}