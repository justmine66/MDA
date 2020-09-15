using System;
using System.Collections.Generic;

namespace MDA.MessageBus
{
    public abstract class Message : IMessage
    {
        protected Message()
            : this(Guid.NewGuid().ToString("N"))
        { }

        protected Message(string id, long? partitionKey = default)
        {
            Id = id;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Items = new Dictionary<string, byte[]>();
            PartitionKey = partitionKey ?? MessagePartitionKeys.GlobalPartitionKey;
        }

        public string Id { get; set; }

        public long Timestamp { get; set; }

        public long PartitionKey { get; set; }

        public IDictionary<string, byte[]> Items { get; set; }
    }

    public abstract class Message<TId> : Message, IMessage<TId>
    {
        protected Message() { }

        protected Message(TId id, long? partitionKey = default) 
            : base(id?.ToString(), partitionKey)
        {
            Id = id;
        }

        public new TId Id { get; set; }
    }
}
