using Newtonsoft.Json;
using System;

namespace MDA.Event.Abstractions.Impl
{
    public class EventSerializer : IEventSerializer
    {
        private readonly static Lazy<EventSerializer> instance = new Lazy<EventSerializer>(() => new EventSerializer(), true);

        public static EventSerializer Instance
        {
            get { return instance.Value; }
        }

        public object Deserialize(string serialization, Type type)
        {
            return JsonConvert.DeserializeObject(serialization, type);
        }

        public T Deserialize<T>(string serialization)
        {
            return JsonConvert.DeserializeObject<T>(serialization);
        }

        public string Serialize(IDomainEvent domainEvent)
        {
            return JsonConvert.SerializeObject(domainEvent);
        }
    }
}
