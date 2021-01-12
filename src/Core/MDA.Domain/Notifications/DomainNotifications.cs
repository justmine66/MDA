using MDA.MessageBus;
using System;

namespace MDA.Domain.Notifications
{
    /// <summary>
    /// 表示一个领域通知
    /// </summary>
    public interface IDomainNotification : IMessage
    {
        /// <summary>
        /// 应用层命令标识
        /// </summary>
        string ApplicationCommandId { get; set; }

        /// <summary>
        /// 应用层命令类型
        /// </summary>
        string ApplicationCommandType { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名称。
        /// </summary>
        Type DomainCommandType { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名称
        /// </summary>
        Type AggregateRootType { get; set; }

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
    /// 领域通知基类
    /// </summary>
    public abstract class DomainNotification : Message, IDomainNotification
    {
        protected DomainNotification() { }
        protected DomainNotification(
            string applicationCommandId, string applicationCommandType,
            string domainCommandId, Type domainCommandType,
            string aggregateRootId, Type aggregateRootType,
            int version = 0)
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            ApplicationCommandId = applicationCommandId;
            ApplicationCommandType = applicationCommandType;
            DomainCommandId = domainCommandId;
            DomainCommandType = domainCommandType;
            AggregateRootId = aggregateRootId;
            AggregateRootType = aggregateRootType;
            Version = version;
        }

        public string ApplicationCommandId { get; set; }
        public string ApplicationCommandType { get; set; }

        public string DomainCommandId { get; set; }
        public Type DomainCommandType { get; set; }

        public string AggregateRootId { get; set; }
        public Type AggregateRootType { get; set; }

        public int Version { get; set; }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainNotification<TAggregateRootId> :
        DomainNotification,
        IDomainNotification<TAggregateRootId>
    {
        protected DomainNotification() { }
        protected DomainNotification(
            string applicationCommandId, string applicationCommandType,
            string domainCommandId, Type domainCommandType,
            TAggregateRootId aggregateRootId, Type aggregateRootType,
            int version = 0)
            : base(applicationCommandId, applicationCommandType,
                domainCommandId, domainCommandType,
            string.Empty, aggregateRootType, 
                version)
        {
            AggregateRootId = aggregateRootId;
        }

        public new TAggregateRootId AggregateRootId { get; set; }
    }
}
