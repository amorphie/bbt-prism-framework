namespace BBT.Prism.Domain.Entites;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}
