using System;

namespace EBank.ApiServer.Infrastructure.Exceptions
{
    /// <summary>
    /// Api层领域异常
    /// </summary>
    public class ApiDomainException : Exception
    {
        public ApiDomainException(string message)
            : base(message) { }
        public ApiDomainException(string message, Exception exception)
            : base(message, exception) { }

        public ApiDomainException(object messages)
        {
            Messages = messages;
        }
        public object Messages { get; set; }
    }
}
