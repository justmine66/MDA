using MDA.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.Messaging.Impl
{
    /// <summary>
    /// 表示一个内存消息总线。
    /// </summary>
    public class InMemoryMessageBus : IMessageBus
    {
        private readonly ILogger<InMemoryMessageBus> _logger;
        private readonly IMessageSubscriberManager _subscriberManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly MessagingOptions _options;

        public InMemoryMessageBus(
            IMessageSubscriberManager subscriberManager,
            ILogger<InMemoryMessageBus> logger,
            IServiceProvider serviceProvider,
            MessagingOptions options)
        {
            _logger = logger;
            _subscriberManager = subscriberManager;
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public string Name { get; } = "InMemoryMessageBus";

        public async Task PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : IMessage
        {
            Assert.NotNull(messages, nameof(messages));

            foreach (var message in messages)
            {
                var messageName = _subscriberManager.GetMessageName<TMessage>();
                await DoPublishAsync(messageName, message);
            }
        }

        public async Task PublishAllDynamicAsync(string messageName, IEnumerable<dynamic> messages)
        {
            Assert.NotNullOrEmpty(messageName, nameof(messageName));
            Assert.NotNull(messages, nameof(messages));

            foreach (var message in messages)
            {
                await DoPublishAsync(messageName, message);
            }
        }

        public async Task PublishAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var messageName = _subscriberManager.GetMessageName<TMessage>();
            await DoPublishAsync(messageName, message);
        }

        public async Task PublishDynamicAsync(string messageName, dynamic message)
        {
            await DoPublishAsync(messageName, message);
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            _subscriberManager.Subscribe<TMessage, TMessageHandler>();
        }

        public void SubscribeDynamic<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            _subscriberManager.SubscribeDynamic<TMessageHandler>(messageName);
        }

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            _subscriberManager.Unsubscribe<TMessage, TMessageHandler>();
        }

        public void UnsubscribeDynamic<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            _subscriberManager.UnsubscribeDynamic<TMessageHandler>(messageName);
        }

        private async Task DoPublishAsync(string messageName, IMessage message)
        {
            Assert.NotNullOrEmpty(messageName, nameof(messageName));
            Assert.NotNull(message, nameof(message));

            var subscribers = _subscriberManager.GetSubscribers(messageName);

            if (subscribers.Any())
            {
                foreach (var subcriber in subscribers)
                {
                    dynamic handler;
                    if (subcriber.IsDynamic)
                    {
                        handler = _serviceProvider.GetService(subcriber.HandlerType) as IDynamicMessageHandler;
                    }
                    else
                    {
                        handler = _serviceProvider.GetService(subcriber.HandlerType) as IMessageHandler;
                    }

                    if (handler != null)
                    {
                        if (_options.WatchSlowMessage)
                        {
                            var start = DateTime.UtcNow;

                            await handler.HandleAsync(message);

                            var elapsed = DateTime.UtcNow - start;
                            if (elapsed > _options.SlowMsgThreshold)
                            {
                                _logger.LogTrace("SLOW BUS MSG [{0}]: {1} - {2}ms. Handler: {3}.", Name, messageName, elapsed.TotalMilliseconds, (string)handler.GetType().Name);
                            }
                        }
                        else
                        {
                            await handler.HandleAsync(message);
                        }

                        _logger.LogInformation($"MESSAGE BUS[{Name}]: the message[{messageName}] had been hanlded[{(string)handler.GetType().Name}].");
                    }
                    else
                    {
                        _logger.LogWarning($"the message[{messageName}] has not the handler.");
                    }
                }
            }
        }
    }
}
