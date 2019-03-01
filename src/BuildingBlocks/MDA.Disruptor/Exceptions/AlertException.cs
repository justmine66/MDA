﻿using System;

namespace MDA.Disruptor.Exceptions
{
    /// <summary p="It does not fill in a stack trace for performance reasons.">
    ///  Used to alert <see cref="IEventProcessor"/>s waiting at a <see cref="ISequenceBarrier"/> of status changes.
    /// </summary>
    public class AlertException: Exception
    {
        /// <summary>
        /// Pre-allocated exception to avoid garbage generation.
        /// </summary>
        public static readonly AlertException Instance = new AlertException();

        /// <summary>
        /// Private constructor so only a single instance exists.
        /// </summary>
        private AlertException()
        {
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
