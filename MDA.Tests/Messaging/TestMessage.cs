using System;
using MDA.Messaging;

namespace MDA.Tests.Messaging
{
    public class TestMessage : IMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
