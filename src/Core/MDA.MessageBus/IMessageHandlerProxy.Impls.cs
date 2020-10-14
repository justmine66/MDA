using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class MessageHandlerProxy<TMessage> : IMessageHandlerProxy<TMessage> 
        where TMessage : class, IMessage
    {
        private readonly IEnumerable<IMessageHandler<TMessage>> _handlers;

        public MessageHandlerProxy(IEnumerable<IMessageHandler<TMessage>> handlers)
        {
            _handlers = handlers;
        }

        public void Handle(IMessage message)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle(message as TMessage);
            }
        }
    }

    public class AsyncMessageHandlerProxy<TMessage> : IAsyncMessageHandlerProxy<TMessage> 
        where TMessage : class, IMessage
    {
        private readonly IEnumerable<IAsyncMessageHandler<TMessage>> _handlers;

        public AsyncMessageHandlerProxy(IEnumerable<IAsyncMessageHandler<TMessage>> handlers)
        {
            _handlers = handlers;
        }

        public async Task HandleAsync(IMessage message, CancellationToken token = default)
        {
            foreach (var handler in _handlers)
            {
                await handler.HandleAsync(message as TMessage, token);
            }
        }
    }
}
