using System;

namespace MDA.Application.Notifications
{
    /// <summary>
    /// 表示一个应用层通知消息
    /// </summary>
    public interface IApplicationNotification
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 时间戳，单位：毫秒。
        /// </summary>
        long Timestamp { get; set; }
    }

    /// <summary>
    /// 表示一个应用层通知消息
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IApplicationNotification<TId> : IApplicationNotification
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }

    /// <summary>
    /// 应用层通知消息基类
    /// </summary>
    public abstract class ApplicationNotification : IApplicationNotification
    {
        protected ApplicationNotification()
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public string Id { get; set; }
        public long Timestamp { get; set; }
    }

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
