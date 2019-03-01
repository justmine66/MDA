using System;
using MDA.Disruptor.Impl;

namespace MDA.Disruptor.Exceptions
{
    /// <summary>
    /// Exception thrown when it is not possible to insert a value into the ring buffer without it wrapping the consuming sequences. Used specifically when claiming with the <see cref="RingBuffer{TEvent}.Next()"/> call.
    /// </summary>
    public class InsufficientCapacityException : Exception
    {
        public static readonly InsufficientCapacityException Instance = new InsufficientCapacityException();

        private InsufficientCapacityException()
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
