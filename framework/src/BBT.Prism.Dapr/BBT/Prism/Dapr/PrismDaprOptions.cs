namespace BBT.Prism.Dapr;

public class PrismDaprOptions
{
    public string? HttpEndpoint { get; set; }

    public string? GrpcEndpoint { get; set; }
    
    public string? DaprApiToken { get; set; }
    
    public string? AppApiToken { get; set; }
}