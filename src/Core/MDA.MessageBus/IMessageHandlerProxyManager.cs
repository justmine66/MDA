using System;

namespace MDA.MessageBus
{
    public interface IMessageHandlerProxyManager
    {
        void InitializeMessageHandlerProxies();

        IMessageHandlerProxy GetMessageHandlerProxy(Type messageHandlerType);

        IAsyncMessageHandlerProxy GetAsyncMessageHandlerProxy(Type messageHandlerType);
    }
}
