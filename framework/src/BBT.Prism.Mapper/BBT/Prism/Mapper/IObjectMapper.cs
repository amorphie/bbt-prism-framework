namespace BBT.Prism.Mapper;

public interface IObjectMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
    void Map<TSource, TDestination>(TSource source, TDestination destination);
}