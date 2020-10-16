using System;

namespace MDA.Shared.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string bytes);

        object Deserialize(string value, Type type);
    }
}
