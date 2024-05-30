namespace BBT.Prism.Logging;

public interface IInitLoggerFactory
{
    IInitLogger<T> Create<T>();
}