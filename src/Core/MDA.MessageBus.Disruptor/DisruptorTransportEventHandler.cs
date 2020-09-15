using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorTransportEventHandler : IEventHandler<DisruptorTransportEvent>
    {
        public void OnEvent(DisruptorTransportEvent data, long sequence, bool endOfBatch)
        {
            var messageType = data.Message.GetType();
            var provider = data.ServiceProvider;
            var manager = provider.GetService<IMessageSubscriber>();
            var logger = provider.GetService<ILogger<DisruptorTransportEventHandler>>();
            var subscribers = manager.GetSubscribers(messageType);

            if (!subscribers.Any())
            {
                logger.LogError($"No message subscriber found for {messageType.FullName}.");

                return;
            }

            foreach (var subscriber in subscribers)
            {
                var messageHandlerType = subscriber.MessageHandlerType;
                var handler = provider.GetService(messageHandlerType);
                if (handler == null)
                {
                    logger.LogError($"No message handler found for {messageType.FullName}.");
                    continue;
                }

                messageHandlerType.GetMethod("Handle")?.Invoke(handler, new object[] { data.Message });
            }
        }
    }
}
