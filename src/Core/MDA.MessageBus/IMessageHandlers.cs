using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }

    public interface IAsyncMessageHandler<in TMessage> where TMessage : IMessage
    {
        Task HandleAsync(TMessage message, CancellationToken token = default);
    }
}
