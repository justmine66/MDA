using System;

namespace MDA.MessageBus.Disruptor
{
    public class MessageRingBufferNotAvailableException : Exception
    {
        public MessageRingBufferNotAvailableException(string message) : base(message) { }
    }
}
