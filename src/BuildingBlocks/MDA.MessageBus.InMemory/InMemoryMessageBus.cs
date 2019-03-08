using MDA.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.MessageBus.InMemory
{
    public class InMemoryMessageBus : IMessageBus, ISlowMessageAware
    {
        private readonly ILogger<InMemoryMessageBus> _logger;
        private readonly IMessageSubscriberManager _subscriberManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly MessageOptions _options;

        public InMemoryMessageBus(
            IMessageSubscriberManager subscriberManager,
            IServiceProvider serviceProvider,
            ILogger<InMemoryMessageBus> logger,
            IOptions<MessageOptions> options)
        {
            _subscriberManager = subscriberManager;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options.Value;
        }


        public string Name { get; } = "InMemoryMessageBus";

        public async Task PublishAsync(string topic, dynamic message)
        {
            await DoPublishAsync(topic, message);
        }

        public async Task PublishAsync<TMessage>(TMessage message)
            where TMessage : Message
        {
            var messageName = _subscriberManager.GetMessageName<TMessage>();
            await DoPublishAsync(messageName, message);
        }

        public async Task PublishAllAsync(string topic, IEnumerable<dynamic> messages)
        {
            Assert.NotNullOrEmpty(topic, nameof(topic));
            Assert.NotNull(messages, nameof(messages));

            foreach (var message in messages)
            {
                await DoPublishAsync(topic, message);
            }
        }

        public async Task PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : Message
        {
            Assert.NotNull(messages, nameof(messages));

            foreach (var message in messages)
            {
                var messageName = _subscriberManager.GetMessageName<TMessage>();
                await DoPublishAsync(messageName, message);
            }
        }

        public void Subscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler
        {
            _subscriberManager.Subscribe<TMessageHandler>(topic);
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : Message where TMessageHandler : IMessageHandler<TMessage>
        {
            _subscriberManager.Subscribe<TMessage, TMessageHandler>();
        }

        public void UnSubscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler
        {
            _subscriberManager.UnSubscribe<TMessageHandler>(topic);
        }

        public void UnSubscribe<TMessage, TMessageHandler>()
            where TMessage : Message where TMessageHandler : IMessageHandler<TMessage>
        {
            _subscriberManager.UnSubscribe<TMessage, TMessageHandler>();
        }

        private async Task DoPublishAsync(string topic, Message message)
        {
            Assert.NotNullOrEmpty(topic, nameof(topic));
            Assert.NotNull(message, nameof(message));

            var subscribers = _subscriberManager.GetSubscribers(topic);
            if (subscribers != null)
            {
                foreach (var subscriber in subscribers)
                {
                    dynamic handler = _serviceProvider.GetService(subscriber.MessageHandlerType);
                    if (handler != null)
                    {
                        if (_options.MonitorSlowMessageHandler)
                        {
                            var start = DateTime.UtcNow;

                            await handler.HandleAsync(message);

                            var elapsed = DateTime.UtcNow - start;
                            if (elapsed > _options.SlowMessageHandlerThreshold)
                            {
                                var @event = new SlowMessageEventArgs(topic, subscriber.MessageHandlerType.Name,
                                    subscriber.MessageHandlerType.Name, elapsed, message.ToString());

                                _logger.LogTrace($"SLOW MSG: {@event}.");

                                OnSlowMessage?.Invoke(this, @event);
                            }
                        }
                        else
                        {
                            await handler.HandleAsync(message);
                        }

                        _logger.LogInformation($"MESSAGE BUS[{Name}]: the message[{topic}] had been handled[{(string)handler.GetType().Name}].");
                    }
                    else
                    {
                        _logger.LogWarning($"the message[{topic}] has not the handler.");
                    }
                }
            }
        }

        public event EventHandler<SlowMessageEventArgs> OnSlowMessage;
    }
}
