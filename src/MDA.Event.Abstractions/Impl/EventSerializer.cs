using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MDA.Event.Abstractions.Impl
{
    public class EventSerializer : IEventSerializer
    {
        public T Deserialize<T>(string serialization)
        {
            return JsonConvert.DeserializeObject<T>(serialization);
        }

        public object Deserialize(string serialization, Type type)
        {
            return JsonConvert.DeserializeObject(serialization, type);
        }

        public string Serialize(IDomainEvent domainEvent)
        {
            return JsonConvert.SerializeObject(domainEvent);
        }

        public string Serialize(IEnumerable<IDomainEvent> eventStream)
        {
            return JsonConvert.SerializeObject(eventStream);
        }
    }
}
