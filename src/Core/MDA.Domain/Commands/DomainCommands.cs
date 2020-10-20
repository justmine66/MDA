using MDA.MessageBus;
using MDA.Shared.Hashes;
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
        Type ApplicationCommandType { get; set; }

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
        protected DomainCommand()
        {
            AggregateRootId = Guid.NewGuid().ToString("N");
        }
        protected DomainCommand(
            string aggregateRootId,
            Type aggregateRootType,
            int version = 0)
        {
            Version = version;
            AggregateRootId = aggregateRootId;
            AggregateRootType = aggregateRootType;
            PartitionKey = MurMurHash3.Hash(aggregateRootType.FullName);
            Version = version;
        }

        protected DomainCommand(
            string applicationCommandId,
            Type applicationCommandType,
            string aggregateRootId,
            Type aggregateRootType,
            int version = 0) : this(aggregateRootId, aggregateRootType, version)
        {
            ApplicationCommandId = applicationCommandId;
            ApplicationCommandType = applicationCommandType;
        }

        public long Version { get; set; }
        public string ApplicationCommandId { get; set; } = string.Empty;
        public Type ApplicationCommandType { get; set; }
        public string AggregateRootId { get; set; }
        public Type AggregateRootType { get; set; }
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
            => base.AggregateRootId = AggregateRootId?.ToString();

        protected DomainCommand(
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int version = 0)
            : base(aggregateRootId?.ToString(),
                aggregateRootType,
                version) => AggregateRootId = aggregateRootId;
        protected DomainCommand(
            string applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int version = 0)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId?.ToString(),
                aggregateRootType,
                version) => AggregateRootId = aggregateRootId;

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
    {
        protected DomainCommand()
        {
            AggregateRootType = typeof(TAggregateRoot);
        }

        protected DomainCommand(
            TAggregateRootId aggregateRootId,
            int version = 0)
            : base(aggregateRootId,
                typeof(TAggregateRoot),
                version) => AggregateRootId = aggregateRootId;

        protected DomainCommand(
            string applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            int version = 0)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId,
                typeof(TAggregateRoot),
                version) => AggregateRootId = aggregateRootId;
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    public abstract class DomainCommand<TAggregateRoot, TId, TAggregateRootId> :
        DomainCommand<TAggregateRoot, TAggregateRootId>,
        IDomainCommand<TId, TAggregateRootId>
    {
        protected DomainCommand() { }
        protected DomainCommand(
            TId id,
            string applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            int version = 0)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId,
                version) => Id = id;

        public new TId Id { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TApplicationCommandId">应用层命令标识类型</typeparam>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    public abstract class DomainCommand<TAggregateRoot, TApplicationCommandId, TId, TAggregateRootId> :
        DomainCommand<TAggregateRoot, TId, TAggregateRootId>,
        IDomainCommand<TApplicationCommandId, TId, TAggregateRootId>
    {
        protected DomainCommand() { }
        protected DomainCommand(
            TId id,
            TApplicationCommandId applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            int version = 0)
            : base(id,
                applicationCommandId.ToString(),
                applicationCommandType,
                aggregateRootId,
                version) => ApplicationCommandId = applicationCommandId;

        private TApplicationCommandId _applicationCommandId;
        public new TApplicationCommandId ApplicationCommandId
        {
            get => _applicationCommandId;
            set
            {
                _applicationCommandId = value;

                base.ApplicationCommandId = _applicationCommandId.ToString();
            }
        }
    }
}
