using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace MDA.MessageBus
{
    public class DefaultMessagePublisherFactory : IMessagePublisherFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<MessageBusClientNames, Lazy<IMessageQueueService>> _activeMessageQueueService;
        private readonly ConcurrentDictionary<MessageBusClientNames, Lazy<IAsyncMessageQueueService>> _activeAsyncMessageQueueService;

        public DefaultMessagePublisherFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _activeMessageQueueService = new ConcurrentDictionary<MessageBusClientNames, Lazy<IMessageQueueService>>();
            _activeAsyncMessageQueueService = new ConcurrentDictionary<MessageBusClientNames, Lazy<IAsyncMessageQueueService>>();
        }

        public IMessagePublisher CreateMessagePublisher(MessageBusClientNames name)
        {
            if (!_activeMessageQueueService.TryGetValue(name, out var messageQueueService))
            {
                var queueService = _serviceProvider.GetServices<IMessageQueueService>()
                    .SingleOrDefault(it => it.Name == name);

                messageQueueService = _activeMessageQueueService[name] = new Lazy<IMessageQueueService>(() => queueService);
            }

            if (!_activeAsyncMessageQueueService.TryGetValue(name, out var asyncMessageQueueService))
            {
                var asyncQueueService = _serviceProvider.GetServices<IAsyncMessageQueueService>()
                    .SingleOrDefault(it => it.Name == name);

                asyncMessageQueueService = _activeAsyncMessageQueueService[name] = new Lazy<IAsyncMessageQueueService>(() => asyncQueueService);
            }

            return new DefaultMessagePublisher(messageQueueService?.Value, asyncMessageQueueService?.Value);
        }
    }
}
