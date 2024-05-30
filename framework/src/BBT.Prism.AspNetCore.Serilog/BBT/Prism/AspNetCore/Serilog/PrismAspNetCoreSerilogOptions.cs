using System.Collections.Generic;

namespace BBT.Prism.AspNetCore.Serilog;

public class PrismAspNetCoreSerilogOptions
{
    public List<string> Headers { get; } =
        ["x-device-id", "x-request-id", "x-token-id", "instanceid", "user-reference"];

    public List<string> Wildcards { get; } = ["authorization", "password"];
    public bool ShouldBodyBeTracked { get; set; } = false;

    public void AddHeader(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!Headers.Contains(key))
            {
                Headers.Add(key);
            }
        }
    }

    public void AddWildcard(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!Wildcards.Contains(key))
            {
                Wildcards.Add(key);
            }
        }
    }
}