using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessagePublisher
    {
        void Publish(IMessage message);

        Task PublishAsync(IMessage message, CancellationToken token);
    }
}
