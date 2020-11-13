using System;

namespace MDA.Infrastructure.Serialization
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T obj, params string[] ignoreKeys);

        T Deserialize<T>(byte[] bytes);

        object Deserialize(byte[] bytes, Type type);
    }
}
