using System;

namespace MDA.Event.Abstractions
{
    public interface IEventSerializer
    {
        T Deserialize<T>(string serialization);
        object Deserialize(string serialization, Type type);
        string Serialize(IDomainEvent domainEvent);
    }
}
