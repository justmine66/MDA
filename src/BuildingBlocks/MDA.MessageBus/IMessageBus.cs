﻿namespace MDA.MessageBus
{
    /// <summary>
    /// 表示一个消息总线。
    /// </summary>
    public interface IMessageBus : IMessagePublisher,
        IMessageSubscriber
    {
        string Name { get; }
    }
}
