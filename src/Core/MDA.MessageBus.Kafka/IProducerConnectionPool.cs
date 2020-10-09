using Confluent.Kafka;

namespace MDA.MessageBus.Kafka
{
    public interface IProducerConnectionPool
    {
        string ServersAddress { get; }

        IProducer<string, byte[]> RentProducer();

        bool Return(IProducer<string, byte[]> producer);
    }
}
