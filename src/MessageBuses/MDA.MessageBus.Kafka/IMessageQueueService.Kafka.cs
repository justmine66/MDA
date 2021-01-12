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

            _logger.LogWarning("Not supported.");
        }

        public void Enqueue(IMessage message)
        {
            // ignore

            _logger.LogWarning("Not supported.");
        }

        public void Stop()
        {
            // ignore
        }
    }
}
