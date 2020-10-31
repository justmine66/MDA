using System;

namespace MDA.Infrastructure.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string bytes);

        object Deserialize(string value, Type type);
    }
}
