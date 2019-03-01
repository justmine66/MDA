using System;

namespace MDA.Disruptor.Exceptions
{
    public class RuntimeException : Exception
    {
        public RuntimeException()
        { }

        public RuntimeException(string message)
            : base(message)
        { }

        public RuntimeException(Exception exception)
            : base(string.Empty, exception)
        { }

        public RuntimeException(string message, Exception exception)
            : base(message, exception)
        { }
    }
}
