using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorTransportEventHandler : IEventHandler<DisruptorTransportEvent>
    {
        public void OnEvent(DisruptorTransportEvent data, long sequence, bool endOfBatch)
        {
            var messageType = data.Message.GetType();
            var serviceProvider = data.ServiceProvider;
            var subscriberManager = serviceProvider.GetService<IMessageSubscriberManager>();
            var logger = serviceProvider.GetService<ILogger<DisruptorTransportEventHandler>>();
            var subscribers = subscriberManager.GetSubscribers(messageType);

            if (!subscribers.Any())
            {
                logger.LogError($"No message subscriber found for {messageType.FullName}.");

                return;
            }

            var handlerProxyManager = serviceProvider.GetService<IMessageHandlerProxyManager>();

            foreach (var subscriber in subscribers)
            {
                var messageHandlerType = subscriber.MessageHandlerType;

                if (subscriber.IsAsynchronousMessageHandler)
                {
                    var handlerProxy = handlerProxyManager.GetAsyncMessageHandlerProxy(messageHandlerType);

                    handlerProxy.HandleAsync(data.Message, CancellationToken.None).GetAwaiter().GetResult();
                }
                else
                {
                    var handlerProxy = handlerProxyManager.GetMessageHandlerProxy(messageHandlerType);

                    handlerProxy.Handle(data.Message);
                }
            }
        }
    }
}
