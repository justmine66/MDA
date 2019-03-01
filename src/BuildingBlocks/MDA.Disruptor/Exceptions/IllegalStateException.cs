using System;

namespace MDA.Disruptor.Exceptions
{
    public class IllegalStateException : Exception
    {
        public IllegalStateException()
        { }

        public IllegalStateException(string message)
            : base(message)
        { }

        public IllegalStateException(Exception exception)
            : base(string.Empty, exception)
        { }

        public IllegalStateException(string message, Exception exception)
            : base(message, exception)
        { }
    }
}
