using System;

namespace BBT.Prism.Dapr;

public interface IDaprSerializer
{
    byte[] Serialize(object obj);

    string SerializeToString(object obj);

    object Deserialize(byte[] value, Type type);

    object Deserialize(string value, Type type);
}
