using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace MDA.Infrastructure.Logging
{
    public class InternalLoggerFactory
    {
        static ILoggerFactory _defaultFactory;

        /// <summary>
        /// Gets or sets the default factory.
        /// </summary>
        public static ILoggerFactory DefaultFactory
        {
            get
            {
                var factory = Volatile.Read(ref _defaultFactory);
                if (factory != null) return factory;

                factory = new LoggerFactory();

                factory.AddProvider(new InternalLoggerProvider());

                var current = Interlocked.CompareExchange(ref _defaultFactory, factory, null);

                return current ?? factory;
            }
            set => Volatile.Write(ref _defaultFactory, value);
        }

        /// <summary>
        ///     Creates a new logger instance with the name of the specified type.
        /// </summary>
        /// <typeparam name="T">type where logger is used</typeparam>
        /// <returns>logger instance</returns>
        public static ILogger GetInstance<T>() => GetInstance(typeof(T));

        /// <summary>
        ///     Creates a new logger instance with the name of the specified type.
        /// </summary>
        /// <param name="type">type where logger is used</param>
        /// <returns>logger instance</returns>
        public static ILogger GetInstance(Type type) => GetInstance(type.FullName);

        /// <summary>
        ///     Creates a new logger instance with the specified name.
        /// </summary>
        /// <param name="name">logger name</param>
        /// <returns>logger instance</returns>
        public static ILogger GetInstance(string name) => DefaultFactory.CreateLogger(name);
    }
}
