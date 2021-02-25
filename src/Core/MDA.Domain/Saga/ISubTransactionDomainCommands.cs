using MDA.Domain.Shared.Commands;

namespace MDA.Domain.Saga
{
    /// <summary>
    /// 表示一个子事务领域命令
    /// </summary>
    public interface ISubTransactionDomainCommand : IDomainCommand
    {
    }

    /// <summary>
    /// 表示一个子事务领域命令
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface ISubTransactionDomainCommand<TAggregateRootId> :
        ISubTransactionDomainCommand,
        IDomainCommand<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个开始位置的子事务领域命令
    /// </summary>
    public interface IBeginSubTransactionDomainCommand : ISubTransactionDomainCommand
    {

    }

    /// <summary>
    /// 表示一个开始位置的子事务领域命令
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface IBeginSubTransactionDomainCommand<TAggregateRootId> :
        IBeginSubTransactionDomainCommand,
        ISubTransactionDomainCommand<TAggregateRootId>
    {

    }

    /// <summary>
    /// 表示一个结束位置的子事务领域命令
    /// </summary>
    public interface IEndSubTransactionDomainCommand : ISubTransactionDomainCommand
    {
        string Message { get; set; }
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域命令
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface IEndSubTransactionDomainCommand<TAggregateRootId> : 
        IEndSubTransactionDomainCommand, 
        ISubTransactionDomainCommand<TAggregateRootId>
    {

    }
}
