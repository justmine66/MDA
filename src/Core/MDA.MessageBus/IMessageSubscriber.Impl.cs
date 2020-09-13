using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.MessageBus
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly Dictionary<Type, List<MessageSubscriberInfo>> _state;

        public MessageSubscriber()
        {
            _state = new Dictionary<Type, List<MessageSubscriberInfo>>();
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage 
            where TMessageHandler : IMessageHandler<TMessage>
        {
            var messageType = typeof(TMessage);
            var subscriber = new MessageSubscriberInfo(messageType, typeof(TMessageHandler));

            if (_state.TryGetValue(messageType, out var handlers))
            {
                handlers.Add(subscriber);
            }
            else
            {
                _state[messageType] = new List<MessageSubscriberInfo>() { subscriber };
            }
        }

        public IEnumerable<MessageSubscriberInfo> GetMessageSubscribers(Type messageType)
            => _state.TryGetValue(messageType, out var handlers) ? handlers : Enumerable.Empty<MessageSubscriberInfo>();
    }
}
