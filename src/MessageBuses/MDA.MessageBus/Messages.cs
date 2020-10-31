namespace MDA.MessageBus
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
        /// 时间戳，单位：毫秒。
        /// </summary>
        long Timestamp { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        string Topic { get; set; }

        /// <summary>
        /// 分区键
        /// </summary>
        int PartitionKey { get; set; }
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
}
