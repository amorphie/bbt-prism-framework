using System;
using AutoMapper;
using AutoMapper.Internal;
using BBT.Prism.Mapper;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.AutoMapper;

[Modules(typeof(
    PrismMapperModule))]
public class PrismAutoMapperModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<IObjectMapper, AutoObjectMapperAdapter>();
        context.Services.AddSingleton(typeof(IObjectMapper<,>), typeof(AutoObjectMapperAdapter<,>));

        context.Services.AddSingleton<IConfigurationProvider>(sp =>
        {
            using var scope = sp.CreateScope();
            var options = scope.ServiceProvider.GetRequiredService<IOptions<PrismAutoMapperOptions>>().Value;

            var mapperConfigurationExpression = sp.GetRequiredService<IOptions<MapperConfigurationExpression>>().Value;
            var autoMapperConfigurationContext =
                new PrismAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider);

            foreach (var configurator in options.Configurators)
            {
                configurator(autoMapperConfigurationContext);
            }

            var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression);

            foreach (var profileType in options.ValidatingProfiles)
            {
                mapperConfiguration.Internal()
                    .AssertConfigurationIsValid(((Profile)Activator.CreateInstance(profileType)!).ProfileName);
            }

            return mapperConfiguration;
        });

        context.Services.AddTransient<IMapper>(sp =>
            sp.GetRequiredService<IConfigurationProvider>().CreateMapper(sp.GetService));
    }
}