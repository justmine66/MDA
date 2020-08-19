using MDA.Messages;

namespace MDA.Application.Notifications
{
    /// <summary>
    /// 表示一个应用层通知消息
    /// </summary>
    public interface IApplicationNotification : IMessage
    { }

    /// <summary>
    /// 表示一个应用层通知消息
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IApplicationNotification<TId> : IApplicationNotification, IMessage<TId> { }

    /// <summary>
    /// 应用层通知消息基类
    /// </summary>
    public abstract class ApplicationNotification : Message, IApplicationNotification { }

    /// <summary>
    /// 应用层通知消息基类
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public abstract class ApplicationNotification<TId> : ApplicationNotification, IApplicationNotification<TId>
    {
        protected ApplicationNotification() { }
        protected ApplicationNotification(TId id) => Id = id;

        public new TId Id { get; set; }
    }
}
