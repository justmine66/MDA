using MDA.MessageBus;
using System;

namespace MDA.Domain.Shared.Events
{
    /// <summary>
    /// 表示一个领域事件
    /// </summary>
    public interface IDomainEvent : IMessage
    {
        /// <summary>
        /// 版本号
        /// 每一个聚合根的领域事件版本号彼此隔离，且单调线性递增。
        /// </summary>
        long Version { get; set; }

        /// <summary>
        /// 应用层命令标识
        /// </summary>
        string ApplicationCommandId { get; set; }

        /// <summary>
        /// 应用层命令类型
        /// </summary>
        string ApplicationCommandType { get; set; }

        /// <summary>
        /// 应用层命令回复方案
        /// </summary>
        ApplicationCommandReplySchemes ApplicationCommandReplyScheme { get; set; }

        /// <summary>
        /// 领域命令标识
        /// </summary>
        string DomainCommandId { get; set; }

        /// <summary>
        /// 领域命令类型
        /// </summary>
        Type DomainCommandType { get; set; }

        /// <summary>
        /// 领域命令版本
        /// </summary>
        long DomainCommandVersion { get; set; }

        /// <summary>
        /// 聚合根标识
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型
        /// </summary>
        Type AggregateRootType { get; set; }

        /// <summary>
        /// 聚合根版本
        /// </summary>
        long AggregateRootVersion { get; set; }

        /// <summary>
        /// 第几代聚合根,随着检查点(Checkpoint)快照而递增.
        /// </summary>
        int AggregateRootGeneration { get; set; }
    }

    /// <summary>
    /// 表示一个领域事件
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainEvent<TAggregateRootId>
        : IDomainEvent
    {
        /// <summary>
        /// 聚合根标识
        /// </summary>
        new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 表示一个领域事件
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">事件标识类型</typeparam>
    public interface IDomainEvent<TAggregateRootId, TId>
        : IDomainEvent<TAggregateRootId>, IMessage<TId>
    { }

    /// <summary>
    /// 表示一个领域事件
    /// </summary>
    /// <typeparam name="TDomainCommandId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">事件标识类型</typeparam>
    public interface IDomainEvent<TDomainCommandId, TAggregateRootId, TId> : IDomainEvent<TAggregateRootId, TId>
    {
        /// <summary>
        /// 领域命令标识
        /// </summary>
        new TDomainCommandId DomainCommandId { get; set; }
    }

    /// <summary>
    /// 领域事件基类
    /// </summary>
    public abstract class DomainEvent : Message, IDomainEvent
    {
        protected DomainEvent() { }
        protected DomainEvent(
            string domainCommandId,
            Type domainCommandType,
            string aggregateRootId,
            Type aggregateRootType,
            int aggregateRootVersion,
            int aggregateRootGeneration,
            int version = 0) : this()
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Version = version;
            DomainCommandId = domainCommandId;
            DomainCommandType = domainCommandType;
            AggregateRootId = aggregateRootId;
            AggregateRootType = aggregateRootType;
            AggregateRootVersion = aggregateRootVersion;
            AggregateRootGeneration = aggregateRootGeneration;
        }
        
        public string DomainCommandId { get; set; } = string.Empty;
        public Type DomainCommandType { get; set; }
        public long DomainCommandVersion { get; set; }

        public string AggregateRootId { get; set; } = string.Empty;
        public Type AggregateRootType { get; set; }
        public long AggregateRootVersion { get; set; }
        public int AggregateRootGeneration { get; set; }

        public long Version { get; set; }

        public string ApplicationCommandId { get; set; }
        public string ApplicationCommandType { get; set; }
        public ApplicationCommandReplySchemes ApplicationCommandReplyScheme { get; set; }
    }

    /// <summary>
    /// 领域事件基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainEvent<TAggregateRootId> :
        DomainEvent,
        IDomainEvent<TAggregateRootId>
    {
        protected DomainEvent()
        {
            base.AggregateRootId = AggregateRootId?.ToString();
        }

        protected DomainEvent(
            string domainCommandId,
            Type domainCommandType,
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int aggregateRootVersion,
            int version = 0)
            : base(domainCommandId,
            domainCommandType,
            aggregateRootId?.ToString(),
            aggregateRootType,
            aggregateRootVersion,
            version)
        {
            AggregateRootId = aggregateRootId;
        }

        private TAggregateRootId _aggregateRootId;
        public new TAggregateRootId AggregateRootId
        {
            get => _aggregateRootId;
            set
            {
                _aggregateRootId = value;

                base.AggregateRootId = _aggregateRootId.ToString();
            }
        }
    }

    /// <summary>
    /// 领域事件基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">领域事件标识类型</typeparam>
    public abstract class DomainEvent<TAggregateRootId, TId> :
        DomainEvent<TAggregateRootId>,
        IDomainEvent<TAggregateRootId, TId>
    {
        protected DomainEvent()
        {
            base.Id = Id?.ToString();
        }
        protected DomainEvent(
            TId id,
            string domainCommandId,
            Type domainCommandType,
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int aggregateRootVersion,
            int version = 0)
            : base(domainCommandId,
                domainCommandType,
                aggregateRootId,
                aggregateRootType,
                aggregateRootVersion,
                version)
        {
            Id = id;
        }

        private TId _id;
        public new TId Id
        {
            get => _id;
            set
            {
                _id = value;

                base.Id = _id.ToString();
            }
        }
    }

    /// <summary>
    /// 领域事件基类
    /// </summary>
    /// <typeparam name="TDomainCommandId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TId">领域事件标识类型</typeparam>
    public abstract class DomainEvent<TDomainCommandId, TAggregateRootId, TId> :
        DomainEvent<TAggregateRootId, TId>,
        IDomainEvent<TDomainCommandId, TAggregateRootId, TId>
    {
        protected DomainEvent()
        {
            base.DomainCommandId = DomainCommandId?.ToString();
        }
        protected DomainEvent(
            TId id,
            TDomainCommandId domainCommandId,
            Type domainCommandType,
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int aggregateRootVersion,
            int version = 0)
            : base(id,
                domainCommandId?.ToString(),
                domainCommandType,
                aggregateRootId,
                aggregateRootType,
                aggregateRootVersion,
                version)
        {
            DomainCommandId = domainCommandId;
        }

        private TDomainCommandId _domainCommandId;
        public new TDomainCommandId DomainCommandId
        {
            get => _domainCommandId;
            set
            {
                _domainCommandId = value;

                base.DomainCommandId = _domainCommandId.ToString();
            }
        }
    }
}