using MDA.Domain.Events;

namespace MDA.Domain.Saga
{
    public class SubTransactionDomainEvent :
        DomainEvent,
        ISubTransactionDomainEvent
    {
    }

    public class SubTransactionDomainEvent<TAggregateRootId> :
        DomainEvent<TAggregateRootId>,
        ISubTransactionDomainEvent<TAggregateRootId>
    {
    }

    public class BeginSubTransactionDomainEvent :
        DomainEvent,
        IBeginSubTransactionDomainEvent
    {
    }

    public class BeginSubTransactionDomainEvent<TAggregateRootId> :
        DomainEvent<TAggregateRootId>,
        IBeginSubTransactionDomainEvent<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域事件
    /// </summary>
    public class EndSubTransactionDomainEvent :
        DomainEvent,
        IEndSubTransactionDomainEvent
    {
        public string Message { get; set; }
    }

    public class EndSubTransactionDomainEvent<TAggregateRootId> :
        DomainEvent<TAggregateRootId>,
        IEndSubTransactionDomainEvent<TAggregateRootId>
    {
        public string Message { get; set; }
    }
}
