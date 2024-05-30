namespace BBT.Prism.DependencyInjection;

public interface IObjectAccessor<out T>
{
    T? Value { get; }
}