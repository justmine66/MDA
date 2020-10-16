using System;
using Newtonsoft.Json;

namespace MDA.Shared.Serialization
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T obj)
            => JsonConvert.SerializeObject(obj);

        public T Deserialize<T>(string value)
            => JsonConvert.DeserializeObject<T>(value);

        public object Deserialize(string value, Type type)
            => JsonConvert.DeserializeObject(value, type);
    }
}
