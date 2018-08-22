namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个消息订阅者。
    /// </summary>
    public interface IMessageSubscriber
    {
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler;

        void SubscribeDynamic<TMessageHandler>(string messageName)
            where TMessageHandler : IDynamicMessageHandler;

        void Unsubscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler;

        void UnsubscribeDynamic<TMessageHandler>(string messageName)
            where TMessageHandler : IDynamicMessageHandler;
    }
}
