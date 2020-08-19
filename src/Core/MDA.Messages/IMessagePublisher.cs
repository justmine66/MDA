using System.Threading.Tasks;

namespace MDA.Messages
{
    public interface IMessagePublisher
    {
        void Publish(IMessage message);
    }

    public interface IAsyncMessagePublisher : IMessagePublisher
    {
        Task PublishAsync(IMessage message);
    }
}
