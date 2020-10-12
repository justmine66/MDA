using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

                var method = typeof(IMessageHandler<>)
                    .MakeGenericType(messageType)
                    .GetMethod("Handle",
                        BindingFlags.Instance | BindingFlags.Public,
                    null,
                    new[] { messageType },
                    null);
                if (method == null)
                {
                    logger.LogError($"No method: Handle was found in {messageHandlerType.FullName}.");
                    continue;
                }

                // 1. 反射
                // method.Invoke(handler, new object[] { data.Message });

                // 2. 表达式树
                var parameter = Expression.Parameter(messageType, "message");
                var call = Expression.Call(Expression.Constant(handler), method, parameter);
                var lambda = Expression.Lambda(call, parameter);
                var methodDelegate = lambda.Compile();

                methodDelegate.DynamicInvoke(data.Message);
            }
        }
    }
}
