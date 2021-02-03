using MDA.Domain;
using MDA.MessageBus;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 表示一条应用层命令消息
    /// </summary>
    public interface IApplicationCommand : IMessage
    {
        /// <summary>
        /// 返回方案，默认：当应用层命令被发送后，返回执行结果，即应用层不关系。
        /// </summary>
        ApplicationCommandResultReturnSchemes ReturnScheme { get; set; }
    }

    /// <summary>
    /// 表示一条应用层命令消息
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IApplicationCommand<TId> : IApplicationCommand, IMessage<TId> { }

    /// <summary>
    /// 应用层命令消息基类
    /// </summary>
    public abstract class ApplicationCommand : Message, IApplicationCommand
    {
        /// <summary>
        /// 返回方案，默认：当应用层命令被发送后，返回执行结果，即应用层不关系。
        /// </summary>
        public ApplicationCommandResultReturnSchemes ReturnScheme { get; set; } = ApplicationCommandResultReturnSchemes.None;
    }
}
