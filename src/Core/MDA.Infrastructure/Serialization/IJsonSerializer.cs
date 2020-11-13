using System;

namespace MDA.Infrastructure.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<TPayload>(TPayload obj, params string[] ignoreKeys);

        TPayload Deserialize<TPayload>(string json);

        object Deserialize(string json, Type type);
    }
}
