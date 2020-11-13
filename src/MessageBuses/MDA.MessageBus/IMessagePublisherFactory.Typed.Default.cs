using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace MDA.MessageBus
{
    public class DefaultTypedMessagePublisherFactory<TMessagePublisher> : ITypedMessagePublisherFactory<TMessagePublisher>
    {
        private static readonly Func<ObjectFactory> CreateActivator = () => ActivatorUtilities.CreateFactory(typeof(TMessagePublisher), new[] { typeof(IMessagePublisher) });

        private readonly IServiceProvider _services;

        private ObjectFactory _activator;
        private bool _initialized;
        private object _lock;

        public DefaultTypedMessagePublisherFactory(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public TMessagePublisher CreateMessagePublisher(IMessagePublisher publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException(nameof(publisher));
            }

            LazyInitializer.EnsureInitialized(ref _activator, ref _initialized, ref _lock, CreateActivator);

            return (TMessagePublisher)_activator(_services, new object[] { publisher });
        }
    }
}
