using System;

namespace MDA.MessageBus
{
    public class MessagePublishFailedException : Exception
    {
        public MessagePublishFailedException(string message) : base(message) { }

        public MessagePublishFailedException(string message, Exception e) : base(message, e) { }
    }
}
 