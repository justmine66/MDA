using System;

namespace MDA.Event.Abstractions
{
    public interface IEventSerializer
    {
        string Serialize(IDomainEvent domainEvent);
        object Deserialize(string serialization, Type type);
        T Deserialize<T>(string serialization);
    }
}
