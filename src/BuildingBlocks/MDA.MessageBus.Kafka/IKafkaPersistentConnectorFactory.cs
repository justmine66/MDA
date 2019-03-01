namespace MDA.MessageBus.Kafka
{
    public interface IKafkaPersistentConnectorFactory
    {
        IKafkaPersistentConnector Create(ChannelType type);
    }
}
