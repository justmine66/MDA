using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessagePublisher
    {
        void Publish(IMessage message);

        void Publish<TPayload>(IMessage<TPayload> message);

        void Publish<TId, TPayload>(IMessage<TId, TPayload> message);
    }

    public interface IAsyncMessagePublisher
    {
        Task PublishAsync(IMessage message);

        Task PublishAsync<TPayload>(IMessage<TPayload> message);

        Task PublishAsync<TId, TPayload>(IMessage<TId, TPayload> message);
    }
}
