using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus
{
    public static class MessageHandlerUtils
    {
        public static void DynamicInvokeHandle(IEnumerable<object> handlerProxies, IMessage message, ILogger logger)
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

                methodDelegate(message);
            }
        }

        public static async Task DynamicInvokeAsyncHandle(IEnumerable<object> handlerProxies, IMessage message, ILogger logger)
        {
            var proxyMessageType = typeof(IMessage);
            var tokenType = typeof(CancellationToken);
            var tokenParameter = Expression.Parameter(tokenType, "token");
            var messageParameter = Expression.Parameter(proxyMessageType, "message");
            var methodName = "HandleAsync";

            foreach (var proxy in handlerProxies)
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

                await methodDelegate(message, CancellationToken.None);
            }
        }
    }
}
