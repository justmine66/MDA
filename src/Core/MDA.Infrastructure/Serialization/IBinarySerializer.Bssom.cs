using Bssom.Serializer;
using System;

namespace MDA.Infrastructure.Serialization
{
    public class BssomBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize<T>(T obj, params string[] ignoreKeys)
        {
            return BssomSerializer.Serialize(obj);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            return BssomSerializer.Deserialize<T>(bytes);
        }

        public object Deserialize(byte[] bytes, Type type)
        {
            var result = BssomSerializer.Deserialize(bytes, 0, out var readSize, type);
            if (readSize != bytes.Length)
            {
                throw new BssomSerializationException($"Deserializing {type.FullName} has a error, expected buffer size: {bytes.Length}, actual buffer size: {readSize}.");
            }

            return result;
        }
    }
}
