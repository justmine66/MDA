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
        int Version { get; set; }

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

        /// <summary>
        /// 有效载荷
        /// </summary>
        object Payload { get; set; }
    }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public interface IDomainCommand<TPayload>
        : IDomainCommand
    {
        /// <summary>
        /// 有效载荷
        /// </summary>
        new TPayload Payload { get; set; }
    }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public interface IDomainCommand<TAggregateRootId, TPayload>
        : IDomainCommand<TPayload>
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
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public interface IDomainCommand<TId, TAggregateRootId, TPayload>
        : IDomainCommand<TAggregateRootId, TPayload>, IMessage<TId>
    { }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TApplicationCommandId">应用层命令标识类型</typeparam>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public interface IDomainCommand<TApplicationCommandId, TId, TAggregateRootId, TPayload>
        : IDomainCommand<TId, TAggregateRootId, TPayload>
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
            int version = 1)
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
            int version = 1) : this(aggregateRootId, aggregateRootType, version)
        {
            ApplicationCommandId = applicationCommandId;
            ApplicationCommandType = applicationCommandType;
        }

        public int Version { get; set; }
        public string ApplicationCommandId { get; set; } = string.Empty;
        public Type ApplicationCommandType { get; set; }
        public string AggregateRootId { get; set; }
        public Type AggregateRootType { get; set; }
        public object Payload { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public abstract class DomainCommand<TPayload> :
        DomainCommand,
        IDomainCommand<TPayload>
    {
        protected DomainCommand() { }

        protected DomainCommand(
            string aggregateRootId,
            Type aggregateRootType,
            int version = 1)
            : base(aggregateRootId,
                aggregateRootType,
                version) => AggregateRootId = aggregateRootId;

        protected DomainCommand(
            string applicationCommandId,
            Type applicationCommandType,
            string aggregateRootId,
            Type aggregateRootType,
            int version = 1)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId,
                aggregateRootType,
                version) => AggregateRootId = aggregateRootId;

        public new TPayload Payload { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public abstract class DomainCommand<TAggregateRootId, TPayload> :
        DomainCommand<TPayload>,
        IDomainCommand<TAggregateRootId, TPayload>
    {
        protected DomainCommand()
            => base.AggregateRootId = AggregateRootId?.ToString();

        protected DomainCommand(
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int version = 1)
            : base(aggregateRootId?.ToString(),
                aggregateRootType,
                version) => AggregateRootId = aggregateRootId;
        protected DomainCommand(
            string applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            int version = 1)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId?.ToString(),
                aggregateRootType,
                version) => AggregateRootId = aggregateRootId;

        public new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public abstract class DomainCommand<TAggregateRoot, TAggregateRootId, TPayload> 
        : DomainCommand<TAggregateRootId, TPayload>,
            IDomainCommand<TAggregateRootId, TPayload>
    {
        protected DomainCommand()
        {
            AggregateRootType = typeof(TAggregateRoot);
        }

        protected DomainCommand(
            TAggregateRootId aggregateRootId,
            int version = 1)
            : base(aggregateRootId,
                typeof(TAggregateRoot),
                version) => AggregateRootId = aggregateRootId;

        protected DomainCommand(
            string applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            int version = 1)
            : base(applicationCommandId,
                applicationCommandType,
                aggregateRootId,
                typeof(TAggregateRoot),
                version) => AggregateRootId = aggregateRootId;

        public new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    /// <typeparam name="TPayload">有效载荷类型</typeparam>
    public abstract class DomainCommand<TAggregateRoot, TId, TAggregateRootId, TPayload> :
        DomainCommand<TAggregateRoot, TAggregateRootId, TPayload>,
        IDomainCommand<TId, TAggregateRootId, TPayload>
    {
        protected DomainCommand() { }
        protected DomainCommand(
            TId id,
            string applicationCommandId,
            Type applicationCommandType,
            TAggregateRootId aggregateRootId,
            int version = 1)
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
            int version = 1)
            : base(id,
                applicationCommandId.ToString(),
                applicationCommandType,
                aggregateRootId,
                version) => ApplicationCommandId = applicationCommandId;

        public new TApplicationCommandId ApplicationCommandId { get; set; }
    }
}
