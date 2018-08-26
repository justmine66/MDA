using System;
using MDA.Messaging;

namespace MDA.Tests.Messaging
{
    public class TestMessage : IMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return string.Format("TestMessage [Id: {0}, Timestamp: {1}]", Id, Timestamp);
        }
    }
}
