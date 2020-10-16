using System;
using MessagePack;

namespace MDA.Shared.Serialization
{
    public class MessagePackBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize<T>(T obj) 
            => MessagePackSerializer.Serialize(obj);

        public T Deserialize<T>(byte[] bytes)
            => MessagePackSerializer.Deserialize<T>(bytes);

        public object Deserialize(byte[] bytes, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
