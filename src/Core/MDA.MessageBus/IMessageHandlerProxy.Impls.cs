using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessageHandlerProxy<TMessage> : IMessageHandlerProxy where TMessage : class, IMessage
    {
        private readonly IMessageHandler<TMessage> _handler;

        public MessageHandlerProxy(IMessageHandler<TMessage> handler)
        {
            _handler = handler;
        }

        public void Handle(IMessage message)
        {
            _handler.Handle(message as TMessage);
        }
    }

    public class AsyncMessageHandlerProxy<TMessage> : IAsyncMessageHandlerProxy where TMessage : class, IMessage
    {
        private readonly IAsyncMessageHandler<TMessage> _handler;

        public AsyncMessageHandlerProxy(IAsyncMessageHandler<TMessage> handler)
        {
            _handler = handler;
        }


        public async Task HandleAsync(IMessage message, CancellationToken token = default)
        {
            await _handler.HandleAsync(message as TMessage, token);
        }
    }
}
