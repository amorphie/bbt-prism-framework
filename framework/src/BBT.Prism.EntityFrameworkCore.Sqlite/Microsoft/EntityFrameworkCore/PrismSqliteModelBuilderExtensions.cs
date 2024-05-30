using BBT.Prism.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class PrismSqliteModelBuilderExtensions
{
    public static void UseSqlite(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.Sqlite);
    }
}