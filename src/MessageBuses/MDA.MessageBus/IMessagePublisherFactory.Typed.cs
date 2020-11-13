namespace MDA.MessageBus
{
    public interface ITypedMessagePublisherFactory<out TMessagePublisher>
    {
        TMessagePublisher CreateMessagePublisher(IMessagePublisher publisher);
    }
}
