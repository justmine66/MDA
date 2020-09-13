using System.Collections.Generic;

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
        /// 分区键
        /// </summary>
        long PartitionKey { get; set; }

        /// <summary>
        /// 消息载荷
        /// </summary>
        object Payload { get; set; }

        /// <summary>
        /// 补充项列表
        /// </summary>
        IDictionary<string, byte[]> Items { get; set; }
    }

    /// <summary>
    /// 表示一条消息
    /// </summary>
    /// <typeparam name="TPayload">消息载荷类型</typeparam>
    public interface IMessage<TPayload>: IMessage
    {
        /// <summary>
        /// 消息载荷
        /// </summary>
        new TPayload Payload { get; set; }
    }

    /// <summary>
    /// 表示一条消息
    /// </summary>
    /// <typeparam name="TId">消息标识类型</typeparam>
    /// <typeparam name="TPayload">消息载荷类型</typeparam>
    public interface IMessage<TId, TPayload> : IMessage<TPayload>
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }
}
