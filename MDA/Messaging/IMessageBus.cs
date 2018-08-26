using System;

namespace MDA.Messaging
{
    /// <summary>
    /// 表示一个消息总线。
    /// </summary>
    public interface IMessageBus :
        IMessagePublisher,
        IMessageSubscriber
    {
        string Name { get; }

        event EventHandler<SlowMessageHandlerEventArgs> OnSlowMessageHandled;
    }
}
