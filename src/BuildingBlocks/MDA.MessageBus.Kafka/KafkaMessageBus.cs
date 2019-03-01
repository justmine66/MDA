using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public class KafkaMessageBus : IMessageBus
    {
        private readonly IMessagePublisher _publisher;
        private readonly IMessageSubscriber _subscriber;

        public KafkaMessageBus(
            ILogger<KafkaMessageBus> logger,
            IMessagePublisher publisher,
            IMessageSubscriber subscriber)
        {
            _publisher = publisher;
            _subscriber = subscriber;
        }

        public string Name => "KafkaMessageBus";

        public async Task PublishAsync(string topic, dynamic message)
        {
            await _publisher.PublishAsync(topic, message);
        }

        public async Task PublishAsync<TMessage>(TMessage message)
            where TMessage : Message
        {
            await _publisher.PublishAsync(message);
        }

        public async Task PublishAllAsync(string topic, IEnumerable<dynamic> messages)
        {
            await _publisher.PublishAllAsync(topic, messages);
        }

        public async Task PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : Message
        {
            await _publisher.PublishAllAsync(messages);
        }

        public void Subscribe<TMessageHandler>(string topic)
            where TMessageHandler : IDynamicMessageHandler
        {
            _subscriber.Subscribe<TMessageHandler>(topic);
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>
        {
            _subscriber.Subscribe<TMessage, TMessageHandler>();
        }

        public void UnSubscribe<TMessageHandler>(string topic) where TMessageHandler : IDynamicMessageHandler
        {
            _subscriber.UnSubscribe<TMessageHandler>(topic);
        }

        public void UnSubscribe<TMessage, TMessageHandler>()
            where TMessage : Message
            where TMessageHandler : IMessageHandler<TMessage>
        {
            _subscriber.UnSubscribe<TMessage, TMessageHandler>();
        }
    }
}
