using Kooboo.Json;
using System;

namespace MDA.Infrastructure.Serialization
{
    public class KoobooJsonSerializer : IJsonSerializer
    {
        public string Serialize<TPayload>(TPayload obj, params string[] ignoreKeys)
        {
            return JsonSerializer.ToJson(obj);
        }

        public TPayload Deserialize<TPayload>(string json)
        {
            return JsonSerializer.ToObject<TPayload>(json);
        }

        public object Deserialize(string json, Type type)
        {
            return JsonSerializer.ToObject(json, type);
        }
    }
}
