namespace MDA.MessageBus.Kafka
{
    public class KafkaMessageQueueService : IMessageQueueService
    {
        public MessageBusClientNames Name => MessageBusClientNames.Kafka;

        public void Start()
        {
            // ignore
        }

        public void Enqueue(IMessage message)
        {
            // ignore
        }

        public void Stop()
        {
            // ignore
        }
    }
}
