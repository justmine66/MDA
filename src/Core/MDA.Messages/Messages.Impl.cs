using System;

namespace MDA.Messages
{
    public abstract class Message : IMessage
    {
        protected Message()
            : this(Guid.NewGuid().ToString("N"))
        { }

        protected Message(string id)
        {
            Id = id;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public string Id { get; set; }

        public long Timestamp { get; set; }
    }

    public abstract class Message<TId> : Message, IMessage<TId>
    {
        protected Message() { }
        protected Message(TId id) : base(string.Empty)
        {
            Id = id;
        }

        public new TId Id { get; set; }
    }

    public abstract class StreamedMessage : Message<string>, IStreamedMessage
    {
        protected StreamedMessage() { }
        protected StreamedMessage(string id, long offset)
            : base(id)
        {
            Offset = offset;
        }

        public long Offset { get; set; }
    }

    public abstract class StreamedMessage<TId> : StreamedMessage, IStreamedMessage<TId>
    {
        protected StreamedMessage() { }
        protected StreamedMessage(TId id, long offset)
            : base(string.Empty, offset)
        {
            Id = id;
        }

        public new TId Id { get; set; }
    }
}
