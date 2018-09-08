using System;

namespace MDA.EventStore.MySql.Poes
{
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

        public override string ToString()
        {
            return $"StoredDomainEvent [EventId: {EventId},EventSequence: {EventSequence},CommandId: {CommandId},AggregateRootId: {AggregateRootId},AggregateRootTypeName: {AggregateRootTypeName},OccurredOn: {OccurredOn},EventBody: {EventBody}]";
        }
    }
}
