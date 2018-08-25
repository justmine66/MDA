using MDA.Messaging;
using MDA.Messaging.Impl;
using Xunit;

namespace MDA.Tests.Messaging
{
    /// <summary>
    /// 内存订阅管理器测试类。
    /// </summary>
    public class InMemory_Message_Subscriber_Manager_Test
    {
        private readonly IMessageSubscriberManager _subscriberManager = new InMemoryMessageSubscriberManager(new DefaultMessageSubscriberCollection());

        [Fact(DisplayName = "第一次创建，订阅者应该为空")]
        public void After_Creation_Should_Be_Empty()
        {
            var subscriberManager = new InMemoryMessageSubscriberManager(new DefaultMessageSubscriberCollection());

            Assert.True(_subscriberManager.IsEmpty);
        }

        [Fact(DisplayName = "添加一个订阅者后，应该包含该订阅者。")]
        public void After_One_Subscriber_Should_Contain_The_Subscriber()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");

            Assert.True(_subscriberManager.HasSubscriber<TestMessage>());
            Assert.True(_subscriberManager.HasSubscriber("TestDynamicMessageHandler"));
        }
    }
}
