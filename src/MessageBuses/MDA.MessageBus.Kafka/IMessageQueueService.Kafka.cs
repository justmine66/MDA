using Microsoft.Extensions.Logging;

namespace MDA.MessageBus.Kafka
{
    public class KafkaMessageQueueService : IMessageQueueService
    {
        private readonly ILogger<KafkaMessageQueueService> _logger;

        public KafkaMessageQueueService(ILogger<KafkaMessageQueueService> logger)
        {
            _logger = logger;
        }

        public MessageBusClientNames Name => MessageBusClientNames.Kafka;

        public void Start()
        {
            // ignore
            _logger.LogWarning($"The method: {nameof(IMessagePublisher)}.{nameof(IMessagePublisher.Publish)} Will ignore all message, please use method: {nameof(IMessagePublisher)}.{nameof(IMessagePublisher.PublishAsync)}.");
        }

        public void Enqueue(IMessage message)
        {
            // ignore

            _logger.LogWarning($"The method: {nameof(IMessagePublisher)}.{nameof(IMessagePublisher.Publish)} Will be not publish message, {message.Topic}, {message.Id}, , please use method: {nameof(IMessagePublisher)}.{nameof(IMessagePublisher.PublishAsync)}.");
        }

        public void Stop()
        {
            // ignore
        }
    }
}
