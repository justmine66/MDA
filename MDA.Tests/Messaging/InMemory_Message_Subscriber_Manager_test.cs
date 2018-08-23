using MDA.Messaging.Impl;
using Xunit;

namespace MDA.Tests.Messaging
{
    /// <summary>
    /// 内存订阅管理器测试类。
    /// </summary>
    public class InMemory_Message_Subscriber_Manager_Test
    {
        /// <summary>
        /// 第一次创建，订阅者应该为空。
        /// </summary>
        [Fact]
        public void After_Creation_Should_Be_Empty()
        {
            var manager = new InMemoryMessageSubscriberManager();

            Assert.True(manager.IsEmpty);
        }

        /// <summary>
        /// 添加一个订阅者后，应该包含该订阅者。
        /// </summary>
        [Fact]
        public void After_One_Subscriber_Should_Contain_The_Subscriber()
        {
            var manager = new InMemoryMessageSubscriberManager();
            manager.Subscribe<TestMessage, TestMessageHandler>();
            manager.SubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");

            Assert.True(manager.HasSubscriber<TestMessage>());
            Assert.True(manager.HasSubscriber("TestDynamicMessageHandler"));
        }
    }
}
