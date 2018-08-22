using MDA.Common;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.Messaging.Impl
{
    /// <summary>
    /// 表示一个内存消息总线。
    /// </summary>
    public class InMemoryMessageBus : IMessageBus
    {
        private readonly ILogger<InMemoryMessageBus> _logger;
        private readonly IMessageSubscriberManager _subscriberManager;

        public InMemoryMessageBus(
            IMessageSubscriberManager subscriberManager,
            ILogger<InMemoryMessageBus> logger)
        {
            _logger = logger;
            _subscriberManager = subscriberManager;
        }

        public string Name { get; } = "InMemoryMessageBus";

        public async Task HandleAsync(IMessage message)
        {
            await PublishAsync(message);
        }

        public Task HandleAsync(dynamic message)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AsyncResult> PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : IMessage
        {
            Assert.NotNull(messages, nameof(messages));

            foreach (var message in messages)
            {
                var messageName = _subscriberManager.GetMessageName<TMessage>();
                await DoPublishAsync(messageName, message);
            }

            return await Task.FromResult(AsyncResult.Success);
        }

        public async Task<AsyncResult> PublishAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            Assert.NotNull<IMessage>(message, nameof(message));

            var messageName = _subscriberManager.GetMessageName<TMessage>();
            await DoPublishAsync(messageName, message);

            return await Task.FromResult(AsyncResult.Success);
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            _subscriberManager.Subscribe<TMessage, TMessageHandler>();
        }

        public void SubscribeDynamic<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            _subscriberManager.SubscribeDynamic<TMessageHandler>(messageName);
        }

        public void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
        {
            _subscriberManager.Unsubscribe<TMessage, TMessageHandler>();
        }

        public void UnsubscribeDynamic<TMessageHandler>(string messageName) where TMessageHandler : IDynamicMessageHandler
        {
            _subscriberManager.UnsubscribeDynamic<TMessageHandler>(messageName);
        }

        private async Task DoPublishAsync(string messageName, IMessage message)
        {
            Assert.NotNull(message, nameof(message));

            var subscribers = _subscriberManager.GetHandlersForMessage(messageName);

            if (subscribers.Any())
            {
                foreach (var subcriber in subscribers)
                {
                    if (subcriber.IsDynamic)
                    {
                        var handler = null as IDynamicMessageHandler;
                        await handler.HandleAsync(message);
                    }
                    else
                    {
                        var handler = null as IMessageHandler;
                        await handler.HandleAsync(message);
                    }
                }
            }
        }
    }
}
