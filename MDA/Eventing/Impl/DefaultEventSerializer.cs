using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MDA.Eventing.Impl
{
    public class DefaultEventSerializer : IEventSerializer
    {
        private readonly static Lazy<DefaultEventSerializer> instance = new Lazy<DefaultEventSerializer>(() => new DefaultEventSerializer(), true);
        //如果为真，则序列化的Json带缩进，可读性比较好。
        private readonly bool isPretty;

        public static DefaultEventSerializer Instance
        {
            get { return instance.Value; }
        }

        public DefaultEventSerializer(bool isPretty = false)
        {
            this.isPretty = isPretty;
        }

        public T Deserialize<T>(string serialization)
        {
            return JsonConvert.DeserializeObject<T>(serialization);
        }

        public object Deserialize<T>(string serialization, Type type)
        {
            return JsonConvert.DeserializeObject(serialization, type);
        }

        public string Serialize(IDomainEvent domainEvent)
        {
            return JsonConvert.SerializeObject(domainEvent, this.isPretty ? Formatting.Indented : Formatting.None);
        }

        public string Serialize(IEnumerable<IDomainEvent> domainEvents)
        {
            return JsonConvert.SerializeObject(domainEvents, this.isPretty ? Formatting.Indented : Formatting.None);
        }
    }
}
