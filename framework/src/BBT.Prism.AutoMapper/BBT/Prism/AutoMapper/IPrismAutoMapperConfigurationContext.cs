using System;
using AutoMapper;

namespace BBT.Prism.AutoMapper;

public interface IPrismAutoMapperConfigurationContext
{
    IMapperConfigurationExpression MapperConfiguration { get; }

    IServiceProvider ServiceProvider { get; }
}