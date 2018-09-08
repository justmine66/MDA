using System;

namespace MDA.Message.Abstractions
{
    /// <summary>
    /// 表示一个消息。
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
