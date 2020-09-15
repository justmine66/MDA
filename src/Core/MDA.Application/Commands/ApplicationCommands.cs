using MDA.MessageBus;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 表示一条应用程序命令消息
    /// </summary>
    public interface IApplicationCommand : IMessage { }

    /// <summary>
    /// 表示一条应用程序命令消息
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IApplicationCommand<TId> : IApplicationCommand, IMessage<TId> { }

    /// <summary>
    /// 应用程序命令消息基类
    /// </summary>
    public abstract class ApplicationCommand : Message, IApplicationCommand
    {
        protected ApplicationCommand() { }
        protected ApplicationCommand(string id, long? partitionKey = default)
            : base(id, partitionKey) { }
    }

    /// <summary>
    /// 应用程序命令消息基类
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public abstract class ApplicationCommand<TId> : Message<TId>, IApplicationCommand<TId>
    {
        protected ApplicationCommand() { }
        protected ApplicationCommand(TId id, long? partitionKey = default)
            : base(id, partitionKey) { }
    }
}
