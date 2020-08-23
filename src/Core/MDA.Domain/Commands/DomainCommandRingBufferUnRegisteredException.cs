using System;

namespace MDA.Domain.Commands
{
    public class DomainCommandRingBufferUnRegisteredException : Exception
    {
        public DomainCommandRingBufferUnRegisteredException(string message) : base(message) { }

        public DomainCommandRingBufferUnRegisteredException(string message, Exception e) : base(message, e) { }
    }
}
