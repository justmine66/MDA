using System;

namespace MDA.Domain.Commands
{
    public class DomainCommandPublishFailedException : Exception
    {
        public DomainCommandPublishFailedException(string message) : base(message) { }

        public DomainCommandPublishFailedException(string message, Exception e) : base(message, e) { }
    }
}
