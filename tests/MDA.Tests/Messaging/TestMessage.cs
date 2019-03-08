using MDA.MessageBus;
using System;

namespace MDA.Tests.Messaging
{
    public class TestMessage : Message
    {
        public override string ToString()
        {
            return $"TestMessage [Id: {Id}, Timestamp: {Timestamp}]";
        }
    }
}
