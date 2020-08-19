using MDA.Messages;
using System;

namespace MDA.Domain.Commands
{
    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    public interface IDomainCommand : IStreamedMessage
    {
        /// <summary>
        /// 应用层命令标识
        /// </summary>
        string ApplicationCommandId { get; set; }

        /// <summary>
        /// 应用层命令类型完全限定名
        /// </summary>
        string ApplicationCommandTypeFullName { get; set; }

        /// <summary>
        /// 聚合根标识。
        /// </summary>
        string AggregateRootId { get; set; }

        /// <summary>
        /// 聚合根类型完全限定名
        /// </summary>
        string AggregateRootTypeFullName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        int Version { get; set; }
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
    public interface IDomainCommand<TId, TAggregateRootId> :
        IDomainCommand<TAggregateRootId>,
        IStreamedMessage<TId>
    { }

    /// <summary>
    /// 表示一个领域命令
    /// </summary>
    /// <typeparam name="TApplicationCommandId">应用层命令标识类型</typeparam>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public interface IDomainCommand<TApplicationCommandId, TId, TAggregateRootId> :
        IDomainCommand<TAggregateRootId>,
        IStreamedMessage<TId>
    {
        /// <summary>
        /// 应用层命令标识
        /// </summary>
        new TApplicationCommandId ApplicationCommandId { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    public abstract class DomainCommand : StreamedMessage, IDomainCommand
    {
        protected DomainCommand() { }
        protected DomainCommand(
            string applicationCommandId,
            string applicationCommandTypeFullName,
            string aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
        {
            Id = Guid.NewGuid().ToString("N");
            ApplicationCommandId = applicationCommandId;
            ApplicationCommandTypeFullName = applicationCommandTypeFullName;
            AggregateRootId = aggregateRootId;
            AggregateRootTypeFullName = aggregateRootTypeFullName;
            Version = version;
        }

        public string ApplicationCommandId { get; set; }
        public string ApplicationCommandTypeFullName { get; set; }
        public string AggregateRootId { get; set; }
        public string AggregateRootTypeFullName { get; set; }
        public int Version { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainCommand<TAggregateRootId> :
        DomainCommand,
        IDomainCommand<TAggregateRootId>
    {
        protected DomainCommand() { }
        protected DomainCommand(
            string applicationCommandId,
            string applicationCommandTypeFullName,
            TAggregateRootId aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
            : base(applicationCommandId,
                applicationCommandTypeFullName,
                string.Empty,
                aggregateRootTypeFullName,
                version) => AggregateRootId = aggregateRootId;

        public new TAggregateRootId AggregateRootId { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainCommand<TId, TAggregateRootId> :
        DomainCommand<TAggregateRootId>,
        IDomainCommand<TId, TAggregateRootId>
    {
        protected DomainCommand() { }
        protected DomainCommand(
            TId id,
            string applicationCommandId,
            string applicationCommandTypeFullName,
            TAggregateRootId aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
            : base(applicationCommandId,
                applicationCommandTypeFullName,
                aggregateRootId,
                aggregateRootTypeFullName,
                version) => Id = id;

        public new TId Id { get; set; }
    }

    /// <summary>
    /// 领域命令基类
    /// </summary>
    /// <typeparam name="TApplicationCommandId">应用层命令标识类型</typeparam>
    /// <typeparam name="TId">领域命令标识类型</typeparam>
    /// <typeparam name="TAggregateRootId">聚合根标识类型</typeparam>
    public abstract class DomainCommand<TApplicationCommandId, TId, TAggregateRootId> :
        DomainCommand<TId, TAggregateRootId>,
        IDomainCommand<TApplicationCommandId, TId, TAggregateRootId>
    {
        protected DomainCommand() { }
        protected DomainCommand(
            TId id,
            TApplicationCommandId applicationCommandId,
            string applicationCommandTypeFullName,
            TAggregateRootId aggregateRootId,
            string aggregateRootTypeFullName,
            int version = 1)
            : base(id,
                string.Empty,
                applicationCommandTypeFullName,
                aggregateRootId,
                aggregateRootTypeFullName,
                version) => ApplicationCommandId = applicationCommandId;

        public new TApplicationCommandId ApplicationCommandId { get; set; }
    }
}
