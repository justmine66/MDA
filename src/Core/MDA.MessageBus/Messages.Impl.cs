using System;
using System.Collections.Generic;

namespace MDA.MessageBus
{
    public abstract class Message : IMessage
    {
        protected Message()
            : this(Guid.NewGuid().ToString("N"))
        { }

        protected Message(string id)
        {
            Id = id;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Items = new Dictionary<string, byte[]>();
            PartitionKey = MessagePartitionKeys.GlobalPartitionKey;
        }

        public string Id { get; set; }

        public long Timestamp { get; set; }

        public long PartitionKey { get; set; }

        public object Payload { get; set; }

        public IDictionary<string, byte[]> Items { get; set; }
    }

    public abstract class Message<TPayload> : Message, IMessage<TPayload>
    {
        protected Message() { }

        protected Message(string id) : base(id) { }

        public new TPayload Payload { get; set; }
    }

    public abstract class Message<TId, TPayload> : Message<TPayload>, IMessage<TId, TPayload>
    {
        protected Message() { }
        protected Message(TId id) : base(string.Empty)
        {
            Id = id;
        }

        public new TId Id { get; set; }
    }
}
