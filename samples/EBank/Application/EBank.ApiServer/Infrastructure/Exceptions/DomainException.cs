using System;

namespace EBank.ApiServer.Infrastructure.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message) { }
        public DomainException(string message, Exception exception)
            : base(message, exception) { }

        public DomainException(object messages)
        {
            Messages = messages;
        }
        public object Messages { get; set; }
    }
}
