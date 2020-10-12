using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
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
            var messageType = data.Message.GetType();
            var provider = data.ServiceProvider;
            var manager = provider.GetService<IMessageSubscriberManager>();
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

                if (subscriber.IsAsynchronousMessageHandler)
                {
                    var methodName = "HandleAsync";
                    var method = typeof(IAsyncMessageHandler<>)
                       .MakeGenericType(messageType)
                       .GetMethod(methodName,
                           BindingFlags.Instance | BindingFlags.Public,
                           null,
                           new[] { messageType },
                           null);

                    if (method == null)
                    {
                        logger.LogError($"No method: {methodName} was found in {messageHandlerType.FullName}.");
                        continue;
                    }

                    var parameter1 = Expression.Parameter(messageType, "message");
                    var parameter2 = Expression.Parameter(typeof(CancellationToken), "token");
                    var call = Expression.Call(Expression.Constant(handler), method, parameter1, parameter2);
                    var lambda = Expression.Lambda<Func<IMessage, CancellationToken, Task>>(call, parameter1, parameter2);
                    var predicate = lambda.Compile();

                    predicate(data.Message, CancellationToken.None).GetAwaiter().GetResult();
                }
                else
                {
                    var methodName = "Handle";
                    var method = typeof(IMessageHandler<>)
                        .MakeGenericType(messageType)
                        .GetMethod(methodName,
                            BindingFlags.Instance | BindingFlags.Public,
                            null,
                            new[] { messageType },
                            null);

                    if (method == null)
                    {
                        logger.LogError($"No method: {methodName} was found in {messageHandlerType.FullName}.");
                        continue;
                    }

                    var parameter = Expression.Parameter(messageType, "message");
                    var call = Expression.Call(Expression.Constant(handler), method, parameter);
                    var lambda = Expression.Lambda<Action<IMessage>>(call, parameter);
                    var predicate = lambda.Compile();

                    predicate(data.Message);
                }
            }
        }

        public class ConvertMemberToColumnVisitor : ExpressionVisitor
        {
            private readonly Expression _columnOwnerExpression;
            private readonly string _memberOwnerName;

            public ConvertMemberToColumnVisitor(Expression columnOwnerExpression, string memberOwnerName)
            {
                _columnOwnerExpression = columnOwnerExpression;
                _memberOwnerName = memberOwnerName;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var parameterExpression = node.Expression as ParameterExpression;
                if (parameterExpression != null && parameterExpression.Name == _memberOwnerName)
                {
                    return Expression.Convert(Expression.Call(_columnOwnerExpression, typeof(DataRow).GetMethod("get_Item", new[] { typeof(string) }), Expression.Constant(node.Member.Name)),
                        ((PropertyInfo)node.Member).PropertyType);
                }

                return base.VisitMember(node);
            }
        }
    }
}
