using System.Text.Json;

namespace BBT.Prism.Dapr;

public class DaprJsonSerializerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };
}