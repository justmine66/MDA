using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageHandlerProxy
    {
        void Handle(IMessage message);
    }

    public interface IAsyncMessageHandlerProxy
    {
        Task HandleAsync(IMessage message, CancellationToken token = default);
    }
}
