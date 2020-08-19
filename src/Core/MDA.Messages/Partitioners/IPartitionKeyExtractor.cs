namespace MDA.Messages.Partitioners
{
    public interface IPartitionKeyExtractor
    {
        long ExtractPartitionKey<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
