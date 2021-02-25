using MDA.Domain.Shared.Events;

namespace MDA.Domain.Saga
{
    /// <summary>
    /// 表示一个子事务领域事件
    /// </summary>
    public interface ISubTransactionDomainEvent : IDomainEvent
    {
    }

    /// <summary>
    /// 表示一个子事务领域事件
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface ISubTransactionDomainEvent<TAggregateRootId> :
        ISubTransactionDomainEvent,
        IDomainEvent<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个开始位置的子事务领域事件
    /// </summary>
    public interface IBeginSubTransactionDomainEvent : ISubTransactionDomainEvent
    {

    }

    /// <summary>
    /// 表示一个开始位置的子事务领域事件
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface IBeginSubTransactionDomainEvent<TAggregateRootId> :
        IBeginSubTransactionDomainEvent,
        ISubTransactionDomainEvent<TAggregateRootId>
    {

    }

    /// <summary>
    /// 表示一个结束位置的子事务领域事件
    /// </summary>
    public interface IEndSubTransactionDomainEvent : ISubTransactionDomainEvent
    {
        string Message { get; set; }
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域事件
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface IEndSubTransactionDomainEvent<TAggregateRootId> : 
        IEndSubTransactionDomainEvent, 
        ISubTransactionDomainEvent<TAggregateRootId>
    {

    }
}
