using System;

namespace MDA.Messaging
{
    public abstract class Message : IMessage
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
