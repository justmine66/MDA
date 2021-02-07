using Disruptor;
using MDA.Infrastructure.Async;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorTransportEventHandler : IEventHandler<DisruptorTransportEvent>
    {
        public void OnEvent(DisruptorTransportEvent data, long sequence, bool endOfBatch)
        {
            ILogger logger = null;

            try
            {
                var serviceProvider = data.ServiceProvider;
                logger = serviceProvider.GetService<ILogger<DisruptorTransportEventHandler>>();

                using (var scope = data.ServiceProvider.CreateScope())
                {
                    var scopeServiceProvider = scope.ServiceProvider;
                    var messageType = data.Message.GetType();
                    var handlerProxyTypeDefinition = typeof(IMessageHandlerProxy<>);
                    var asyncHandlerProxyTypeDefinition = typeof(IAsyncMessageHandlerProxy<>);

                    var hasHandler = false;
                    var handlerProxies = scopeServiceProvider.GetServices(handlerProxyTypeDefinition.MakeGenericType(messageType));
                    if (handlerProxies.IsNotEmpty())
                    {
                        hasHandler = true;
                        MessageHandlerUtils.DynamicInvokeHandle(handlerProxies, data.Message, logger);
                    }

                    var asyncHandlerProxies = scopeServiceProvider.GetServices(asyncHandlerProxyTypeDefinition.MakeGenericType(messageType));
                    if (asyncHandlerProxies.IsNotEmpty())
                    {
                        hasHandler = true;
                        MessageHandlerUtils.DynamicInvokeAsyncHandle(asyncHandlerProxies, data.Message, logger).SyncRun();
                    }

                    if (!hasHandler)
                    {
                        logger.LogWarning($"No message handler found: {messageType.FullName}.");
                    }
                }
            }
            catch (Exception e)
            {
                var message = $"Handling disruptor transport event has a unknown exception: {e}.";

                if (logger == null)
                {
                    Console.WriteLine(message);

                    return;
                }

                logger.LogError(message);
            }
        }
    }
}
