using System;

namespace MDA.Shared.Serialization
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] bytes);

        object Deserialize(byte[] bytes, Type type);
    }
}
