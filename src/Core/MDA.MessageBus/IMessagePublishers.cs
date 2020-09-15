using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessagePublisher
    {
        void Publish(IMessage message);
    }

    public interface IAsyncMessagePublisher
    {
        Task PublishAsync(IMessage message);
    }
}
