using Confluent.Kafka;

namespace MDA.MessageBus.Kafka
{
    public interface IKafkaConsumerClientFactory
    {
        IConsumer<string, byte[]> CreateClient();
    }
}
