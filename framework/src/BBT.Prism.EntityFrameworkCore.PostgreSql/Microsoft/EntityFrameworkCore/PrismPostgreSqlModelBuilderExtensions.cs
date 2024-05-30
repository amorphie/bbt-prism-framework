using BBT.Prism.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class PrismPostgreSqlModelBuilderExtensions
{
    public static void UsePostgreSql(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.PostgreSql);
    }
}