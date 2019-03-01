﻿using MDA.Common;
using System;

namespace MDA.MessageBus
{
    /// <summary>
    /// 描述一个消息订阅者。
    /// </summary>
    public class MessageSubscriberDescriptor
    {
        /// <summary>
        /// 是否为动态订阅者
        /// </summary>
        public bool IsDynamic { get; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public Type MessageType { get; }
        /// <summary>
        /// 消息处理者类型
        /// </summary>
        public Type MessageHandlerType { get; }

        /// <summary>
        /// 初始化一个 <see cref="MessageSubscriberDescriptor"/> 实例。
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="handlerType">消息处理者类型</param>
        public MessageSubscriberDescriptor(
            string topic,
            Type messageType,
            Type handlerType)
        {
            IsDynamic = messageType == null;
            MessageType = messageType;
            MessageHandlerType = handlerType;
            Topic = topic;
        }

        /// <summary>
        /// 创建一个动态订阅者。
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="handlerType">处理者类型</param>
        /// <returns></returns>
        public static MessageSubscriberDescriptor Dynamic(string topic, Type handlerType)
        {
            return new MessageSubscriberDescriptor(topic, null, handlerType);
        }

        /// <summary>
        /// 创建一个类型化订阅者。
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="handlerType">处理者类型</param>
        /// <returns></returns>
        public static MessageSubscriberDescriptor Typed(Type messageType, Type handlerType)
        {
            return new MessageSubscriberDescriptor(messageType.Name, messageType, handlerType);
        }

        public override bool Equals(object obj)
        {
            return obj is MessageSubscriberDescriptor other &&
                   IsDynamic == other.IsDynamic &&
                   Topic == other.Topic &&
                   MessageType == other.MessageType &&
                   MessageHandlerType == other.MessageHandlerType;
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(new object[] { IsDynamic, Topic, MessageType, MessageHandlerType });
        }
    }
}
