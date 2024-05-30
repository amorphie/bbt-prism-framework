using System;
using BBT.MyProjectName.Data;
using BBT.MyProjectName.Issues.Handlers;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Domain;
using BBT.Prism.Modularity;
using BBT.Prism.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.MyProjectName;

[Modules(
    typeof(PrismDddDomainModule),
    typeof(MyProjectNameDomainSharedModule),
    typeof(PrismDataSeedingModule)
    )]
public class MyProjectNameDomainModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        Configure<ClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
        
        context.Services.AddTransient<MyProjectNameDbMigrationService>();
        context.Services.AddTransient<MyProjectNameDataSeedContributor>();

        context.Services.AddTransient<IssueClosedEventHandler>();
        context.Services.AddTransient<IssueReOpenedEventHandler>();
    }
}
