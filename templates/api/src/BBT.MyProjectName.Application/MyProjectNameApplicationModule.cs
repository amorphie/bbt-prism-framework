using BBT.MyProjectName.Issues;
using BBT.Prism.Application;
using BBT.Prism.AutoMapper;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.MyProjectName;

[Modules(
    typeof(MyProjectNameDomainModule),
    typeof(MyProjectNameApplicationContractsModule),
    typeof(PrismDddApplicationModule),
    typeof(PrismAutoMapperModule)
)]
public class MyProjectNameApplicationModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        Configure<PrismAutoMapperOptions>(options =>
        {
            options.AddMaps<MyProjectNameApplicationModule>(validate: true);
        });
        
        context.Services.AddTransient<IIssueAppService, IssueAppService>();
    }
}