using BBT.Prism.AspNetCore.ExceptionHandling;
using BBT.Prism.Data;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.ExceptionHandling;

[Modules(
    typeof(PrismDataModule)
)]
public class PrismExceptionHandlingModule: PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddTransient<IExceptionToErrorInfoConverter, DefaultExceptionToErrorInfoConverter>();
    }
}