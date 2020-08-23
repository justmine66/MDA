using System;

namespace MDA.Domain.Commands
{
    public class DomainCommandRingBufferNotAvailableException : Exception
    {
        public DomainCommandRingBufferNotAvailableException(string message) : base(message) { }

        public DomainCommandRingBufferNotAvailableException(string message, Exception e) : base(message, e) { }
    }
}
