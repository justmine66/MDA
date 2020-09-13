using System;

namespace MDA.Domain.Notifications
{
    /// <summary>
    /// 表示一个领域通知
    /// </summary>
    public interface IDomainNotification
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 时间戳，单位：毫秒。
        /// </summary>
        long Timestamp { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名称。
        /// </summary>
        string DomainCommandTypeFullName { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名称
        /// </summary>
        string AggregateRootTypeFullName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        int Version { get; set; }
    }

    /// <summary>
    /// 表示一个领域通知
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainNotification<TAggregateRootId> : IDomainNotification
    {
        /// <summary>
        /// 聚合根标识
        /// </summary>
        new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 表示一个领域通知
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">事件标识类型</typeparam>
    public interface IDomainNotification<TAggregateRootId, TId> : IDomainNotification<TAggregateRootId>
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }

    /// <summary>
    /// 表示一个领域通知
    /// </summary>
    /// <typeparam name="TDomainCommandId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">事件标识类型</typeparam>
    public interface IDomainNotification<TDomainCommandId, TAggregateRootId, TId> : IDomainNotification<TAggregateRootId, TId>
    {
        /// <summary>
        /// 领域命令标识
        /// </summary>
        new TDomainCommandId DomainCommandId { get; set; }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    public abstract class DomainNotification : IDomainNotification
    {
        protected DomainNotification() { }
        protected DomainNotification(
            string domainCommandId,
            string domainCommandTypeFullName,
            string aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            DomainCommandId = domainCommandId;
            DomainCommandTypeFullName = domainCommandTypeFullName;
            AggregateRootId = aggregateRootId;
            AggregateRootTypeFullName = aggregateRootTypeFullName;
            Version = version;
        }

        public string Id { get; set; }
        public long Timestamp { get; set; }
        public string DomainCommandId { get; set; }
        public string DomainCommandTypeFullName { get; set; }

        public string AggregateRootId { get; set; }
        public string AggregateRootTypeFullName { get; set; }

        public int Version { get; set; }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainNotification<TAggregateRootId> : DomainNotification, IDomainNotification<TAggregateRootId>
    {
        protected DomainNotification() { }
        protected DomainNotification(
            string domainCommandId,
            string domainCommandTypeFullName,
            TAggregateRootId aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
            : base(domainCommandId,
            domainCommandTypeFullName,
            string.Empty,
            aggregateRootTypeFullName,
            version)
        {
            AggregateRootId = aggregateRootId;
        }

        public new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">领域通知标识类型</typeparam>
    public abstract class DomainNotification<TAggregateRootId, TId> :
        DomainNotification<TAggregateRootId>,
        IDomainNotification<TAggregateRootId, TId>
    {
        protected DomainNotification() { }
        protected DomainNotification(
            TId id,
            string domainCommandId,
            string domainCommandTypeFullName,
            TAggregateRootId aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
            : base(domainCommandId,
                domainCommandTypeFullName,
                aggregateRootId,
                aggregateRootTypeFullName,
                version)
        {
            Id = id;
        }

        public new TId Id { get; set; }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    /// <typeparam name="TDomainCommandId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">领域通知标识类型</typeparam>
    public abstract class DomainNotification<TDomainCommandId, TAggregateRootId, TId> : DomainNotification<TAggregateRootId, TId>, IDomainNotification<TDomainCommandId, TAggregateRootId, TId>
    {
        protected DomainNotification() { }
        protected DomainNotification(
            TId id,
            TDomainCommandId domainCommandId,
            string domainCommandTypeFullName,
            TAggregateRootId aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
            : base(id,
                string.Empty,
                domainCommandTypeFullName,
                aggregateRootId,
                aggregateRootTypeFullName,
                version)
        {
            DomainCommandId = domainCommandId;
        }

        public new TDomainCommandId DomainCommandId { get; set; }
    }
}
