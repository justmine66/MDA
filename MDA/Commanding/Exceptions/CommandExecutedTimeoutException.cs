using System;

namespace MDA.Commanding.Exceptions
{
    public class CommandExecutedTimeoutException : Exception
    {
        public CommandExecutedTimeoutException() : base() { }
        public CommandExecutedTimeoutException(string message) : 
            base(message) { }
        public CommandExecutedTimeoutException(string message, Exception innerException) : 
            base(message, innerException) { }
    }
}
