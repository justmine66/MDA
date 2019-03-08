using Confluent.Kafka;
using MDA.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public class KafkaMessageConsumer : IMessageSubscriber, ISlowMessageAware
    {
        private readonly ILogger _logger;
        private readonly MessageOptions _options;
        private readonly IMessageSubscriberManager _subsManager;
        private readonly IKafkaPersistentConnector _connector;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaMessageConsumer(
            IMessageSubscriberManager subsManager,
            IKafkaPersistentConnectorFactory connectorFactory,
            ILogger<KafkaMessageConsumer> logger,
            IServiceScopeFactory serviceScopeFactory, IOptions<MessageOptions> options)
        {
            _subsManager = subsManager;
            _connector = connectorFactory.Create(ChannelType.Consumer);
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.Value;
        }

        public void Subscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler
        {
            if (!_subsManager.HasSubscriber(topic))
            {
                if (!(_connector.CreateChannel() is Consumer<Null, string> consumer))
                    throw new Exception("Unable to establish producer channel.");

                consumer.Subscribe(topic);
                consumer.OnMessage += OnMessage;
            }

            _subsManager.Subscribe<TMessageHandler>(topic);
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>
        {
            if (!_subsManager.HasSubscriber<TMessage>())
            {
                if (!(_connector.CreateChannel() is Consumer<Null, string> consumer))
                    throw new Exception("Unable to establish producer channel.");

                var topic = typeof(TMessage).Name;
                consumer.Subscribe(topic);
                consumer.OnMessage += OnMessage;
            }

            _subsManager.Subscribe<TMessage, TMessageHandler>();
        }

        public void UnSubscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler
        {
            _subsManager.UnSubscribe<TMessageHandler>(topic);
        }

        public void UnSubscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>
        {
            _subsManager.UnSubscribe<TMessage, TMessageHandler>();
        }

        public event EventHandler<SlowMessageEventArgs> OnSlowMessage;

        private void OnMessage(object sender, Message<Null, string> e)
        {
            ProcessMessage(e.Topic, e.Value).Wait();
        }

        private async Task ProcessMessage(string topic, string message)
        {
            Assert.NotNullOrEmpty(topic, nameof(topic));
            Assert.NotNull(message, nameof(message));

            if (_subsManager.HasSubscriber(topic))
            {
                var subscribers = _subsManager.GetSubscribers(topic);
                if (subscribers != null)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        foreach (var subscriber in subscribers)
                        {
                            dynamic handler = scope.ServiceProvider.GetService(subscriber.MessageHandlerType);

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
                                            subscriber.MessageHandlerType.Name, elapsed, message);

                                        _logger.LogTrace($"SLOW MSG: {@event}.");

                                        OnSlowMessage?.Invoke(this, @event);
                                    }
                                }
                                else
                                {
                                    await handler.HandleAsync(message);
                                }

                                _logger.LogInformation($"MESSAGE: the message[{subscriber.MessageType.Name}] had been handled[{(string)handler.GetType().Name}].");
                            }
                            else
                            {
                                _logger.LogError($"MESSAGE]: the message[{subscriber.MessageType.Name}] handler could not be found.");
                            }
                        }
                    }
                }
            }
        }
    }
}
