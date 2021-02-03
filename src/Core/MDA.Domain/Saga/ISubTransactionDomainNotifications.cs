using MDA.Domain.Notifications;

namespace MDA.Domain.Saga
{
    /// <summary>
    /// 表示一个子事务领域通知
    /// </summary>
    public interface ISubTransactionDomainNotification : IDomainNotification
    {
    }

    /// <summary>
    /// 表示一个子事务领域通知
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface ISubTransactionDomainNotification<TAggregateRootId> : 
        ISubTransactionDomainNotification, 
        IDomainNotification<TAggregateRootId>
    {
    }

    /// <summary>
    /// 表示一个开始位置的子事务领域通知
    /// </summary>
    public interface IBeginSubTransactionDomainNotification : ISubTransactionDomainNotification
    {

    }

    /// <summary>
    /// 表示一个开始位置的子事务领域通知
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface IBeginSubTransactionDomainNotification<TAggregateRootId> :
        IBeginSubTransactionDomainNotification,
        ISubTransactionDomainNotification<TAggregateRootId>
    {

    }

    /// <summary>
    /// 表示一个结束位置的子事务领域通知
    /// </summary>
    public interface IEndSubTransactionDomainNotification : ISubTransactionDomainNotification
    {
        string Message { get; set; }
    }

    /// <summary>
    /// 表示一个结束位置的子事务领域通知
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根类型</typeparam>
    public interface IEndSubTransactionDomainNotification<TAggregateRootId> :
        IEndSubTransactionDomainNotification,
        ISubTransactionDomainNotification<TAggregateRootId>
    {

    }
}
