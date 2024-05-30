using BBT.Prism.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class PrismSqlServerModelBuilderExtensions
{
    public static void UseSqlServer(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.SqlServer);
    }
}