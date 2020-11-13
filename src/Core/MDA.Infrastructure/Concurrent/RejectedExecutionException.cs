using System;

namespace MDA.Infrastructure.Concurrent
{
    public class RejectedExecutionException : Exception
    {
        public RejectedExecutionException()
        {
        }

        public RejectedExecutionException(string message)
            : base(message)
        {
        }
    }
}
