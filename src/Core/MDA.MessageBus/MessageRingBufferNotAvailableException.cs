using System;

namespace MDA.MessageBus
{
    public class MessageRingBufferNotAvailableException : Exception
    {
        public MessageRingBufferNotAvailableException(string message) : base(message) { }

        public MessageRingBufferNotAvailableException(string message, Exception e) : base(message, e) { }
    }
}
