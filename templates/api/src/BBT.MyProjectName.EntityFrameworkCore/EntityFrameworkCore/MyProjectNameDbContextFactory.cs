using System.IO;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Timing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.MyProjectName.EntityFrameworkCore;

public class MyProjectNameDbContextFactory: IDesignTimeDbContextFactory<MyProjectNameDbContext>
{
    public MyProjectNameDbContext CreateDbContext(string[] args)
    {
        // Only design context
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();
        services.AddTransient<IClock, Clock>();
        var serviceProvider = services.BuildServiceProvider();
        
        var builder = new DbContextOptionsBuilder<MyProjectNameDbContext>()
            .UseNpgsql(GetConnectionStringFromConfiguration(), b =>
            {
                b.MigrationsHistoryTable("__MyProjectName_Migrations");
            });
        
        return new MyProjectNameDbContext(serviceProvider, builder.Options);
    }
    
    private static string? GetConnectionStringFromConfiguration()
    {
        return BuildConfiguration()
            .GetConnectionString("Default");
    }
    
    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    $"..{Path.DirectorySeparatorChar}BBT.MyProjectName.HttpApi.Host"
                )
            )
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: false);

        return builder.Build();
    }
}