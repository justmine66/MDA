using MDA.Domain.Models;
using MDA.Infrastructure.Hashes;
using MDA.MessageBus;
using System;

namespace MDA.Domain.Commands
{
    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    public interface IDomainCommand : IMessage
    {
        /// <summary>
        /// 版本
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
        /// 聚合根标识。
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型
        /// </summary>
        Type AggregateRootType { get; set; }
    }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainCommand<TAggregateRootId> : IDomainCommand
    {
        /// <summary>
        /// 聚合根标识，一般为聚合根标识。
        /// </summary>
        new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainCommand<TId, TAggregateRootId>
        : IDomainCommand<TAggregateRootId>, IMessage<TId>
    { }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TApplicationCommandId">应用层命令标识类型</typeparam>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainCommand<TApplicationCommandId, TId, TAggregateRootId>
        : IDomainCommand<TId, TAggregateRootId>
    {
        /// <summary>
        /// 应用层命令标识
        /// </summary>
        new TApplicationCommandId ApplicationCommandId { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    public abstract class DomainCommand : Message, IDomainCommand
    {
        private bool _initialized;
        protected DomainCommand() => Initialize();
        protected DomainCommand(
            string aggregateRootId,
            Type aggregateRootType,
            int version = 0) : this()
        {
            Version = version;
            AggregateRootId = aggregateRootId;
            AggregateRootType = aggregateRootType;
            PartitionKey = MurMurHash3.Hash(aggregateRootType.FullName);
            Version = version;
        }

        protected DomainCommand(
            string applicationCommandId,
            string applicationCommandType,
            string aggregateRootId,
            Type aggregateRootType,
            int version = 0) : this(aggregateRootId, aggregateRootType, version)
        {
            ApplicationCommandId = applicationCommandId;
            ApplicationCommandType = applicationCommandType;
        }

        public long Version { get; set; }
        public string ApplicationCommandId { get; set; } = string.Empty;
        public string ApplicationCommandType { get; set; }

        public ApplicationCommandReplySchemes ApplicationCommandReplyScheme { get; set; } = ApplicationCommandReplySchemes.None;
        public string AggregateRootId { get; set; }
        public Type AggregateRootType { get; set; }

        protected void Initialize()
        {
            if (AggregateRootType == null || _initialized) return;

            Topic = AggregateRootType.FullName;
            PartitionKey = MurMurHash3.Hash(AggregateRootType.FullName);

            _initialized = true;
        }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainCommand<TAggregateRootId> :
        DomainCommand,
        IDomainCommand<TAggregateRootId>
    {
        protected DomainCommand()
        {
            base.AggregateRootId = AggregateRootId?.ToString();
            Initialize();
        }

        protected DomainCommand(
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int version = 0)
            : base(aggregateRootId?.ToString(),
                aggregateRootType,
                version)
        {
            AggregateRootId = aggregateRootId;
            Initialize();
        }
        protected DomainCommand(
            string applicationCommandId,
            string applicationCommandType,
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int version = 0)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId?.ToString(),
                aggregateRootType,
                version)
        {
            AggregateRootId = aggregateRootId;
            Initialize();
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
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainCommand<TAggregateRoot, TAggregateRootId> : DomainCommand<TAggregateRootId>
        where TAggregateRoot : IEventSourcedAggregateRoot
    {
        protected DomainCommand()
        {
            AggregateRootType = typeof(TAggregateRoot);
            Initialize();
        }

        protected DomainCommand(
            TAggregateRootId aggregateRootId,
            int version = 0)
            : base(aggregateRootId,
                typeof(TAggregateRoot),
                version)
        {
            AggregateRootId = aggregateRootId;
            Initialize();
        }

        protected DomainCommand(
            string applicationCommandId,
            string applicationCommandType,
            TAggregateRootId aggregateRootId,
            int version = 0)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId,
                typeof(TAggregateRoot),
                version)
        {
            AggregateRootId = aggregateRootId;
            Initialize();
        }
    }
}
