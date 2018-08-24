using System;

namespace MDA.Messaging
{
    /// <summary>
    /// 消息订阅者信息。
    /// </summary>
    public class MessageSubscriberInfo
    {
        /// <summary>
        /// 是否动态消息
        /// </summary>
        public bool IsDynamic { get; }
        /// <summary>
        /// 处理者类型
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// 初始化一个 <see cref="MessageSubscriberInfo"/> 实例。
        /// </summary>
        /// <param name="isDynamic">是否动态消息</param>
        /// <param name="handlerType">处理者类型</param>
        public MessageSubscriberInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        /// <summary>
        /// 动态订阅者。
        /// </summary>
        /// <param name="handlerType">处理者类型</param>
        /// <returns></returns>
        public static MessageSubscriberInfo Dynamic(Type handlerType)
        {
            return new MessageSubscriberInfo(true, handlerType);
        }

        /// <summary>
        /// 类型化订阅者。
        /// </summary>
        /// <param name="handlerType">处理者类型</param>
        /// <returns></returns>
        public static MessageSubscriberInfo Typed(Type handlerType)
        {
            return new MessageSubscriberInfo(false, handlerType);
        }
    }
}
