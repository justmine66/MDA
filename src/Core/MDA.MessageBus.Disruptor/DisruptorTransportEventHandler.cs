using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorTransportEventHandler : IEventHandler<DisruptorTransportEvent>
    {
        public void OnEvent(DisruptorTransportEvent data, long sequence, bool endOfBatch)
        {
            var serviceProvider = data.ServiceProvider;
            var logger = serviceProvider.GetService<ILogger<DisruptorTransportEventHandler>>();

            using (var scope = data.ServiceProvider.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                var messageType = data.Message.GetType();
                var handlerProxyTypeDefinition = typeof(IMessageHandlerProxy<>);
                var asyncHandlerProxyTypeDefinition = typeof(IAsyncMessageHandlerProxy<>);

                var handlerProxies = scopeServiceProvider.GetServices(handlerProxyTypeDefinition.MakeGenericType(messageType));
                if (handlerProxies != null)
                {
                    var proxyMessageType = typeof(IMessage);
                    var messageParameter = Expression.Parameter(proxyMessageType, "message");
                    var methodName = "Handle";

                    foreach (var proxy in handlerProxies)
                    {
                        var method = proxy.GetType()
                            .GetMethod(methodName,
                                BindingFlags.Instance | BindingFlags.Public,
                                null,
                                new[] { proxyMessageType },
                                null);
                        if (method == null)
                        {
                            logger.LogError($"No method: {methodName} was found in {proxyMessageType.FullName}.");
                            continue;
                        }

                        var call = Expression.Call(Expression.Constant(proxy), method, messageParameter);
                        var lambda = Expression.Lambda<Action<IMessage>>(call, messageParameter);
                        var methodDelegate = lambda.Compile();

                        methodDelegate(data.Message);
                    }
                }

                var asyncHandlerProxies = scopeServiceProvider.GetServices(asyncHandlerProxyTypeDefinition.MakeGenericType(messageType));
                if (asyncHandlerProxies != null)
                {
                    var proxyMessageType = typeof(IMessage);
                    var tokenType = typeof(CancellationToken);
                    var tokenParameter = Expression.Parameter(tokenType, "token");
                    var messageParameter = Expression.Parameter(proxyMessageType, "message");
                    var methodName = "HandleAsync";

                    foreach (var proxy in asyncHandlerProxies)
                    {
                        var method = proxy.GetType()
                            .GetMethod(methodName,
                                BindingFlags.Instance | BindingFlags.Public,
                                null,
                                new[] { proxyMessageType, tokenType },
                                null);
                        if (method == null)
                        {
                            logger.LogError($"No method: {methodName} was found in {proxyMessageType.FullName}.");
                            continue;
                        }

                        var call = Expression.Call(Expression.Constant(proxy), method, messageParameter, tokenParameter);
                        var lambda = Expression.Lambda<Func<IMessage, CancellationToken, Task>>(call, messageParameter, tokenParameter);
                        var methodDelegate = lambda.Compile();

                        methodDelegate(data.Message, CancellationToken.None).GetAwaiter().GetResult();
                    }
                }
            }
        }
    }
}
