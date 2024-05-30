using AutoMapper;
using BBT.Prism.Mapper;

namespace BBT.Prism.AutoMapper;

public class AutoObjectMapperAdapter(IMapper mapper) : IObjectMapper
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return mapper.Map<TSource, TDestination>(source);
    }

    public void Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        mapper.Map(source, destination);
    }
}