using System;
using System.Text;
using System.Text.Json;

namespace BBT.Prism.Dapr;

public class Utf8JsonDaprSerializer : IDaprSerializer
{
    private JsonSerializerOptions JsonSerializerOptions { get; }

    public Utf8JsonDaprSerializer()
    {
        JsonSerializerOptions = CreateJsonSerializerOptions();
    }

    public byte[] Serialize(object obj)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, JsonSerializerOptions));
    }

    public string SerializeToString(object obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }

    public object Deserialize(byte[] value, Type type)
    {
        return JsonSerializer.Deserialize(Encoding.UTF8.GetString(value), type, JsonSerializerOptions)!;
    }

    public object Deserialize(string value, Type type)
    {
        return JsonSerializer.Deserialize(value, type, JsonSerializerOptions)!;
    }

    private JsonSerializerOptions CreateJsonSerializerOptions()
    {
        return new JsonSerializerOptions(new DaprJsonSerializerOptions().JsonSerializerOptions);
    }
}