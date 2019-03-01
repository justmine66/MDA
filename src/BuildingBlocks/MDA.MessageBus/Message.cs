using System;

namespace MDA.MessageBus
{
    /// <summary>
    /// 表示一个消息。
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
