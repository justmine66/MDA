using Disruptor;
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

                    var handlerProxies = scopeServiceProvider.GetServices(handlerProxyTypeDefinition.MakeGenericType(messageType));
                    if (handlerProxies.IsNotEmpty())
                    {
                        MessageHandlerUtils.DynamicInvokeHandle(handlerProxies, data.Message, logger);
                    }

                    var asyncHandlerProxies = scopeServiceProvider.GetServices(asyncHandlerProxyTypeDefinition.MakeGenericType(messageType));
                    if (asyncHandlerProxies.IsNotEmpty())
                    {
                        MessageHandlerUtils.DynamicInvokeAsyncHandle(asyncHandlerProxies, data.Message, logger);
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
