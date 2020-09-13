using System;

namespace MDA.Application.Commands
{
    /// <summary>
    /// 表示一条应用程序命令消息
    /// </summary>
    public interface IApplicationCommand
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
    /// 表示一条应用程序命令消息
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public interface IApplicationCommand<TId> : IApplicationCommand
    {
        /// <summary>
        /// 标识
        /// </summary>
        new TId Id { get; set; }
    }

    /// <summary>
    /// 应用程序命令消息基类
    /// </summary>
    public abstract class ApplicationCommand : IApplicationCommand
    {
        protected ApplicationCommand()
        {
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public string Id { get; set; }
        public long Timestamp { get; set; }
    }

    /// <summary>
    /// 应用程序命令消息基类
    /// </summary>
    /// <typeparam name="TId">标识类型</typeparam>
    public abstract class ApplicationCommand<TId> : ApplicationCommand, IApplicationCommand<TId>
    {
        protected ApplicationCommand() { }
        protected ApplicationCommand(TId id) => Id = id;

        public new TId Id { get; set; }
    }
}
