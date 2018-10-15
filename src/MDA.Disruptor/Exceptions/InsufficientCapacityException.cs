using System;

namespace MDA.Disruptor.Exceptions
{
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
