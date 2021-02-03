using MDA.Domain.Commands;
using MDA.Domain.Models;

namespace MDA.Domain.Saga
{
    public class SubTransactionDomainCommand :
        DomainCommand,
        ISubTransactionDomainCommand
    {
    }

    public class SubTransactionDomainCommand<TAggregateRootId> :
        DomainCommand<TAggregateRootId>,
        ISubTransactionDomainCommand<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个子事务领域命令
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public class SubTransactionDomainCommand<TAggregateRoot, TAggregateRootId> :
        DomainCommand<TAggregateRoot, TAggregateRootId>,
        ISubTransactionDomainCommand<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
    }

    public class BeginSubTransactionDomainCommand :
        DomainCommand,
        IBeginSubTransactionDomainCommand
    {
    }

    public class BeginSubTransactionDomainCommand<TAggregateRootId> :
        DomainCommand<TAggregateRootId>,
        IBeginSubTransactionDomainCommand<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个开始位置的子事务领域命令
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public class BeginSubTransactionDomainCommand<TAggregateRoot, TAggregateRootId> :
        DomainCommand<TAggregateRoot, TAggregateRootId>,
        IBeginSubTransactionDomainCommand<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域命令
    /// </summary>
    public class EndSubTransactionDomainCommand :
        DomainCommand,
        IEndSubTransactionDomainCommand
    {
        public string Message { get; set; }
    }

    public class EndSubTransactionDomainCommand<TAggregateRootId> :
        DomainCommand<TAggregateRootId>,
        IEndSubTransactionDomainCommand<TAggregateRootId>
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域命令
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public class EndSubTransactionDomainCommand<TAggregateRoot, TAggregateRootId> :
        DomainCommand<TAggregateRoot, TAggregateRootId>,
        IEndSubTransactionDomainCommand<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
        public string Message { get; set; }
    }
}
