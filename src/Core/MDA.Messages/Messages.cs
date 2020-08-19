namespace MDA.Messages
{
    /// <summary>
    /// 表示一条消息
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 时间戳，单位：秒。
        /// </summary>
        long Timestamp { get; set; }
    }

    /// <summary>
    /// 表示一条消息
    /// </summary>
    /// <typeparam name="TId">消息标识类型</typeparam>
    public interface IMessage<TId> : IMessage
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }

    /// <summary>
    /// 表示一条流式消息
    /// </summary>
    public interface IStreamedMessage : IMessage<string>
    {
        /// <summary>
        /// 偏移量，即消息位点序号。
        /// </summary>
        long Offset { get; set; }
    }

    /// <summary>
    /// 表示一条流式消息
    /// </summary>
    /// <typeparam name="TId">消息标识类型</typeparam>
    public interface IStreamedMessage<TId> : IStreamedMessage
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }
}
