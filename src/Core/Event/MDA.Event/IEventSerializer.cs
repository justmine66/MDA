using System;
using System.Collections.Generic;

namespace MDA.Event
{
    public interface IEventSerializer
    {
        T Deserialize<T>(string serialization);
        object Deserialize(string serialization, Type type);
        string Serialize(DomainEvent domainEvent);
        string Serialize(IEnumerable<DomainEvent> eventStream);
    }
}
