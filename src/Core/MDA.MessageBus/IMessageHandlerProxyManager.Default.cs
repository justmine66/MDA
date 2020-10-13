using System;
using System.Collections.Concurrent;

namespace MDA.MessageBus
{
    public class MessageHandlerProxyManager : IMessageHandlerProxyManager
    {
        private readonly ConcurrentDictionary<Type, IMessageHandlerProxy> _proxies;
        private readonly ConcurrentDictionary<Type, IAsyncMessageHandlerProxy> _asyncProxies;

        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageSubscriberManager _subscriberManager;

        public MessageHandlerProxyManager(
            IMessageSubscriberManager subscriberManager,
            IServiceProvider serviceProvider)
        {
            _subscriberManager = subscriberManager;
            _serviceProvider = serviceProvider;
            _proxies = new ConcurrentDictionary<Type, IMessageHandlerProxy>();
            _asyncProxies = new ConcurrentDictionary<Type, IAsyncMessageHandlerProxy>();
        }

        public void InitializeMessageHandlerProxies()
        {
            var subscribers = _subscriberManager.GetAllSubscribers();

            if (subscribers == null)
            {
                return;
            }

            foreach (var subscriber in subscribers)
            {
                var handlerType = subscriber.MessageHandlerType;
                var handler = _serviceProvider.GetService(handlerType);

                if (subscriber.IsAsynchronousMessageHandler)
                {
                    var proxyType = typeof(AsyncMessageHandlerProxy<>).MakeGenericType(subscriber.MessageType);

                    _asyncProxies[handlerType] = Activator.CreateInstance(proxyType, handler) as IAsyncMessageHandlerProxy;
                }
                else
                {
                    var proxyType = typeof(MessageHandlerProxy<>).MakeGenericType(subscriber.MessageType);

                    _proxies[handlerType] = Activator.CreateInstance(proxyType, handler) as IMessageHandlerProxy;
                }
            }
        }

        public IMessageHandlerProxy GetMessageHandlerProxy(Type messageHandlerType)
        {
            if (messageHandlerType == null)
            {
                throw new ArgumentException(nameof(messageHandlerType));
            }

            var typeKey = "IMessageHandler";
            if (!messageHandlerType.Name.StartsWith(typeKey))
            {
                throw new NotSupportedException($"The Type: {messageHandlerType.FullName} cannot be implementation of the interface: {typeKey}.");
            }

            return _proxies[messageHandlerType];
        }

        public IAsyncMessageHandlerProxy GetAsyncMessageHandlerProxy(Type messageHandlerType)
        {
            if (messageHandlerType == null)
            {
                throw new ArgumentException(nameof(messageHandlerType));
            }

            var typeKey = "IAsyncMessageHandler";
            if (!messageHandlerType.Name.StartsWith(typeKey))
            {
                throw new NotSupportedException($"The Type: {messageHandlerType.FullName} cannot be implementation of the interface: {typeKey}.");
            }

            return _asyncProxies[messageHandlerType];
        }
    }
}
