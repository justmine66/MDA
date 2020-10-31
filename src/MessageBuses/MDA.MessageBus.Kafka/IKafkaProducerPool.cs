using Confluent.Kafka;

namespace MDA.MessageBus.Kafka
{
    public interface IKafkaProducerPool
    {
        IProducer<string, byte[]> Rent();

        bool Return(IProducer<string, byte[]> producer);
    }
}
