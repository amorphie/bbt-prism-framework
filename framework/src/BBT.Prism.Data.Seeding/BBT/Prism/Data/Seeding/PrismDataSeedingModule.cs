using System;
using System.Collections.Generic;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Data.Seeding;

[Modules(
    typeof(PrismDataModule))]
public class PrismDataSeedingModule: PrismModule
{
    public override void PreConfigureServices(ModuleConfigurationContext context)
    {
        var contributors = new List<Type>();
        context.Services.OnRegistered(registerContext =>
        {
            if (typeof(IDataSeedContributor).IsAssignableFrom(registerContext.ImplementationType))
            {
                contributors.Add(registerContext.ImplementationType);
            }
        });
        
        context.Services.Configure<PrismDataSeedOptions>(options =>
        {
            options.Contributors.AddIfNotContains(contributors);
        });
    }

    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddTransient<IDataSeeder, DataSeeder>();
    }
}