namespace BBT.Prism.AspNetCore.Dapr;

public interface IDaprAppApiTokenValidator
{
    void CheckDaprAppApiToken();

    bool IsValidDaprAppApiToken();

    string? GetDaprAppApiTokenOrNull();
}