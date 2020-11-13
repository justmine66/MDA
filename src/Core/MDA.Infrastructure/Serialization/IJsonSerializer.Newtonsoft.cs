using Newtonsoft.Json;
using System;

namespace MDA.Infrastructure.Serialization
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public string Serialize<TPayload>(TPayload payload, params string[] ignoreKeys)
        {
            return ignoreKeys?.Length > 0
                ? JsonConvert.SerializeObject(payload, new JsonSerializerSettings()
                {
                    ContractResolver = new JsonPropertyContractResolver(ignoreKeys)
                })
                : JsonConvert.SerializeObject(payload);
        }

        public TPayload Deserialize<TPayload>(string json)
            => JsonConvert.DeserializeObject<TPayload>(json);

        public object Deserialize(string json, Type type)
            => JsonConvert.DeserializeObject(json, type);
    }
}
