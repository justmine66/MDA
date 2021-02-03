using MDA.Domain.Models;
using MDA.Infrastructure.Hashes;
using MDA.MessageBus;

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
        /// 应用层命令返回方案
        /// </summary>
        ApplicationCommandResultReturnSchemes ApplicationCommandReturnScheme { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型完全限定名称。
        /// </summary>
        string DomainCommandType { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名称
        /// </summary>
        string AggregateRootType { get; set; }

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
        private bool _initialized;
        protected DomainNotification() => Initialize();

        public string ApplicationCommandId { get; set; }
        public string ApplicationCommandType { get; set; }
        public ApplicationCommandResultReturnSchemes ApplicationCommandReturnScheme { get; set; }

        public string DomainCommandId { get; set; }
        public string DomainCommandType { get; set; }

        public string AggregateRootId { get; set; }
        public string AggregateRootType { get; set; }

        public int Version { get; set; }

        protected void Initialize()
        {
            if (AggregateRootType == null || _initialized) return;

            Topic = AggregateRootType;
            PartitionKey = MurMurHash3.Hash(AggregateRootType);

            _initialized = true;
        }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainNotification<TAggregateRootId> :
        DomainNotification,
        IDomainNotification<TAggregateRootId>
    {
        protected DomainNotification()
        {
            base.AggregateRootId = AggregateRootId?.ToString();
            Initialize();
        }

        public new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 领域通知基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainNotification<TAggregateRoot, TAggregateRootId> : DomainNotification<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
        protected DomainNotification()
        {
            AggregateRootType = typeof(TAggregateRoot).FullName;
            Initialize();
        }
    }
}
