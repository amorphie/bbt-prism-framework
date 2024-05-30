namespace BBT.Prism.Dapr;

public interface IDaprApiTokenProvider
{
    string? GetDaprApiToken();

    string? GetAppApiToken();
}