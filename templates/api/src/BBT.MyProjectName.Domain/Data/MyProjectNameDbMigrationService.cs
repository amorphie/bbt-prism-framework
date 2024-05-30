using System.Threading.Tasks;
using BBT.Prism.Data.Seeding;
using Microsoft.Extensions.Logging;

namespace BBT.MyProjectName.Data;

public class MyProjectNameDbMigrationService(
    ILogger<MyProjectNameDbMigrationService> logger,
    IDataSeeder dataSeeder,
    IMyProjectNameDbSchemaMigrator schemaMigrator)
{
    public async Task MigrateAsync()
    {
        logger.LogInformation("Started database migrations...");
        
        await MigrateDatabaseSchemaAsync();
        await SeedDataAsync();

        logger.LogInformation($"Successfully completed database migrations.");
        logger.LogInformation("You can safely end this process...");
    }
    
    private async Task MigrateDatabaseSchemaAsync()
    {
        logger.LogInformation( 
            $"Migrating schema for database...");
        
        await schemaMigrator.MigrateAsync();
        
        logger.LogInformation( 
            $"Migrated schema for database...");
    }
    
    private async Task SeedDataAsync()
    {
        logger.LogInformation($"Executing database seed...");
        
        await dataSeeder.SeedAsync(new DataSeedContext());
        
        logger.LogInformation($"Executed database seed...");
    }
}