using MDA.Messages;

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
    public interface IApplicationCommand<TId> : IApplicationCommand, IStreamedMessage<TId> { }

    /// <summary>
    /// 应用程序命令消息基类
    /// </summary>
    public abstract class ApplicationCommand : Message, IApplicationCommand { }

    /// <summary>
    /// 应用程序命令消息基类
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public abstract class ApplicationCommand<TId> : ApplicationCommand, IMessage<TId>
    {
        public new TId Id { get; set; }
    }
}
