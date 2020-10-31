using System;

namespace MDA.MessageBus
{
    public abstract class Message : IMessage
    {
        protected Message()
            : this(Guid.NewGuid().ToString("N"))
        { }

        protected Message(string id, int? partitionKey = default)
        {
            Id = id;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            PartitionKey = partitionKey ?? MessagePartitionKeys.GlobalPartitionKey;
            Topic = MessageTopics.GlobalTopic;
        }

        public string Id { get; set; }

        public long Timestamp { get; set; }

        public string Topic { get; set; }

        public int PartitionKey { get; set; }
    }

    public abstract class Message<TId> : Message, IMessage<TId>
    {
        protected Message() => base.Id = Id?.ToString();
        protected Message(TId id, int? partitionKey = default)
            : base(id?.ToString(), partitionKey)
        {
            Id = id;
        }

        public new TId Id { get; set; }
    }
}
