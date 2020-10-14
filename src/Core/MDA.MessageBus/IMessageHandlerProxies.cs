using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public interface IMessageHandlerProxy<TMessage> 
        where TMessage : IMessage
    {
        void Handle(IMessage message);
    }

    public interface IAsyncMessageHandlerProxy<TMessage> 
        where TMessage : IMessage
    {
        Task HandleAsync(IMessage message, CancellationToken token = default);
    }
}
