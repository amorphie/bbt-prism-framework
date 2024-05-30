using System;
using AutoMapper;

namespace BBT.Prism.AutoMapper;

public class PrismAutoMapperConfigurationContext(
    IMapperConfigurationExpression mapperConfigurationExpression,
    IServiceProvider serviceProvider)
    : IPrismAutoMapperConfigurationContext
{
    public IMapperConfigurationExpression MapperConfiguration { get; } = mapperConfigurationExpression;

    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
