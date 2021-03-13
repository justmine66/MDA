using MDA.Domain.Models;
using MDA.Domain.Notifications;

namespace MDA.Domain.Saga
{
    public class SubTransactionDomainNotification :
        DomainNotification,
        ISubTransactionDomainNotification
    {
    }

    public class SubTransactionDomainNotification<TAggregateRootId> :
        DomainNotification<TAggregateRootId>,
        ISubTransactionDomainNotification<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个子事务领域通知
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public class SubTransactionDomainNotification<TAggregateRoot, TAggregateRootId> :
        DomainNotification<TAggregateRoot, TAggregateRootId>,
        ISubTransactionDomainNotification<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
    }

    public class BeginSubTransactionDomainNotification :
        DomainNotification,
        IBeginSubTransactionDomainNotification
    {
    }

    public class BeginSubTransactionDomainNotification<TAggregateRootId> :
        DomainNotification<TAggregateRootId>,
        IBeginSubTransactionDomainNotification<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个开始位置的子事务领域通知
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public class BeginSubTransactionDomainNotification<TAggregateRoot, TAggregateRootId> :
        DomainNotification<TAggregateRoot, TAggregateRootId>,
        IBeginSubTransactionDomainNotification<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域通知
    /// </summary>
    public class EndSubTransactionDomainNotification :
        DomainNotification,
        IEndSubTransactionDomainNotification
    {
        public string Message { get; set; }
    }

    public class EndSubTransactionDomainNotification<TAggregateRootId> :
        DomainNotification<TAggregateRootId>,
        IEndSubTransactionDomainNotification<TAggregateRootId>
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域通知
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public class EndSubTransactionDomainNotification<TAggregateRoot, TAggregateRootId> :
        DomainNotification<TAggregateRoot, TAggregateRootId>,
        IEndSubTransactionDomainNotification<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
        public string Message { get; set; }
    }
}
