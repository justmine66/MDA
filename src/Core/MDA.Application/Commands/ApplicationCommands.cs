using MDA.MessageBus;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 表示一条应用层命令消息
    /// </summary>
    public interface IApplicationCommand : IMessage { }

    /// <summary>
    /// 表示一条应用层命令消息
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IApplicationCommand<TId> : IApplicationCommand, IMessage<TId> { }

    /// <summary>
    /// 应用层命令消息基类
    /// </summary>
    public abstract class ApplicationCommand : Message, IApplicationCommand { }
}
