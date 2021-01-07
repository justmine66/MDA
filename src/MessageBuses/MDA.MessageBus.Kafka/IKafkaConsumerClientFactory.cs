using Confluent.Kafka;

namespace MDA.MessageBus.Kafka
{
    public interface IKafkaConsumerClientFactory
    {
        string ConsumeGroup { get; }

        IConsumer<string, byte[]> CreateClient();
    }
}
