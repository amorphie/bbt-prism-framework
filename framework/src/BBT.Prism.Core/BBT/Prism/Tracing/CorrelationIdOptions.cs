namespace BBT.Prism.Tracing;

public class CorrelationIdOptions
{
    public string HttpHeaderName { get; set; } = "X-Request-Id";

    public bool SetResponseHeader { get; set; } = true;
}