using System;
using MessagePack;

namespace MDA.Infrastructure.Serialization
{
    public class MessagePackBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize<T>(T obj, params string[] ignoreKeys) 
            => MessagePackSerializer.Serialize(obj);

        public T Deserialize<T>(byte[] bytes)
            => MessagePackSerializer.Deserialize<T>(bytes);

        public object Deserialize(byte[] bytes, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
