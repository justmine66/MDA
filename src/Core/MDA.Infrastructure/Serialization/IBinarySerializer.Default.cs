using System;
using MDA.Infrastructure.Utils;

namespace MDA.Infrastructure.Serialization
{
    public class DefaultBinarySerializer : IBinarySerializer
    {
        private readonly IJsonSerializer _jsonSerializer;

        public DefaultBinarySerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public byte[] Serialize<T>(T obj)
        {
            var json = _jsonSerializer.Serialize(obj);

            return BinarySerializationHelper.SerializeString(json);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            var json = BinarySerializationHelper.DeserializeString(bytes);

            return _jsonSerializer.Deserialize<T>(json);
        }

        public object Deserialize(byte[] bytes, Type type)
        {
            var json = BinarySerializationHelper.DeserializeString(bytes);

            return _jsonSerializer.Deserialize(json, type);
        }
    }
}
