using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public class DefaultMessageHandlerProxy<TMessage> : IMessageHandlerProxy<TMessage> 
        where TMessage : class, IMessage
    {
        private readonly IEnumerable<IMessageHandler<TMessage>> _handlers;

        public DefaultMessageHandlerProxy(IEnumerable<IMessageHandler<TMessage>> handlers)
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

    public class DefaultAsyncMessageHandlerProxy<TMessage> : IAsyncMessageHandlerProxy<TMessage> 
        where TMessage : class, IMessage
    {
        private readonly IEnumerable<IAsyncMessageHandler<TMessage>> _handlers;

        public DefaultAsyncMessageHandlerProxy(IEnumerable<IAsyncMessageHandler<TMessage>> handlers)
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
