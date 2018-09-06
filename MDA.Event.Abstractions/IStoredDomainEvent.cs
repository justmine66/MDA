using System;

namespace MDA.Event.Abstractions
{
    /// <summary>
    /// 领域事件持久化对象
    /// </summary>
    public class StoredDomainEvent
    {
        public string EventId { get; set; }
        public string EventBody { get; set; }
        /// <summary>
        /// 事件序列号
        /// </summary>
        public int EventSequence { get; set; }
        public string CommandId { get; set; }
        public string AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
        public DateTime? OccurredOn { get; set; }
    }
}
