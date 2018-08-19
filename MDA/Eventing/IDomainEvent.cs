using MDA.Messaging;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个领域事件。
    /// </summary>
    public interface IDomainEvent: ISequenceMessage
    {

    }
    /// <summary>
    /// 表示一个泛型领域事件。
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainEvent<TAggregateRootId> : IDomainEvent
    {
        /// <summary>
        /// 聚合根标识。
        /// </summary>
        TAggregateRootId AggregateRootId { get; set; }
    }
}
