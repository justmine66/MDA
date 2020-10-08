using MessagePack;

namespace MDA.Shared.Serialization
{
    public class MessagePackBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize<T>(T obj) 
            => MessagePackSerializer.Serialize(obj);

        public T DeSerialize<T>(byte[] bytes)
            => MessagePackSerializer.Deserialize<T>(bytes);
    }
}
