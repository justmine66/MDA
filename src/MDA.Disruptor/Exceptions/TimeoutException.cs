using System;

namespace MDA.Disruptor.Exceptions
{
    public class TimeoutException : Exception
    {
        public static readonly TimeoutException Instance = new TimeoutException();

        private TimeoutException()
        {
            // Singleton
        }

        /// <summary>
        /// throw an exception.
        /// </summary>
        public static void Throw()
        {
            throw Instance;
        }
    }
}
