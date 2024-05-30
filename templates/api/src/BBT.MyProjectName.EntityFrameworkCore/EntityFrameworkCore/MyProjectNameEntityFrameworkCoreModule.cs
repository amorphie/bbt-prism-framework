using BBT.MyProjectName.Data;
using BBT.MyProjectName.Issues;
using BBT.Prism.EntityFrameworkCore;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.MyProjectName.EntityFrameworkCore;

[Modules(
    typeof(MyProjectNameDomainModule),
    typeof(PrismEntityFrameworkCoreModule)
)]
public class MyProjectNameEntityFrameworkCoreModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
       
        context.Services.AddTransient<IMyProjectNameDbSchemaMigrator, MyProjectNameDbSchemaMigrator>();

        /* App repositories */
        context.Services.AddTransient<IGitRepository, EfCoreGitRepository>();
        context.Services.AddTransient<IIssueRepository, EfCoreIssueRepository>();
    }
}