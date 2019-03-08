using MDA.MessageBus;
using System.Linq;
using Xunit;

namespace MDA.Tests.Messaging
{
    /// <summary>
    /// 内存订阅管理器测试类。
    /// </summary>
    public class InMemoryMessageSubscriberManagerTest
    {
        private readonly IMessageSubscriberManager _subscriberManager = new InMemoryMessageSubscriberManager(new MessageSubscriberCollection());

        [Fact(DisplayName = "第一次创建，订阅者应该为空。")]
        public void After_Creation_Should_Be_Empty()
        {
            var subscriberManager = new InMemoryMessageSubscriberManager(new MessageSubscriberCollection());

            Assert.True(subscriberManager.IsEmpty);
        }

        [Fact(DisplayName = "添加一个订阅者后，应该包含该订阅者。")]
        public void After_One_Subscriber_was_Added_Should_Contain_The_Subscriber()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Subscribe<TestDynamicMessageHandler>("TestDynamicMessageHandler");

            Assert.True(_subscriberManager.HasSubscriber<TestMessage>());
            Assert.True(_subscriberManager.HasSubscriber("TestDynamicMessageHandler"));
        }

        [Fact(DisplayName = "添加一个订阅者后，消息处理者应该唯一。")]
        public void After_One_Subscriber_Was_Added_The_Message_Handler_Should_Be_One()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Subscribe<TestDynamicMessageHandler>("TestDynamicMessageHandler");

            var subscribers = _subscriberManager.GetSubscribers<TestMessage>();
            var dynamicSubscribers = _subscriberManager.GetSubscribers("TestDynamicMessageHandler");

            Assert.Single(subscribers);
            Assert.Single(dynamicSubscribers);
        }

        [Fact(DisplayName = "移除所有的订阅者后，订阅者列表应该为空。")]
        public void After_All_Subscriber_Be_Removed_Subscriber_Should_Be_Empty()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler1>();
            _subscriberManager.Subscribe<TestDynamicMessageHandler>("TestDynamicMessageHandler");
            _subscriberManager.Subscribe<TestDynamicMessageHandler1>("TestDynamicMessageHandler");

            _subscriberManager.UnSubscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.UnSubscribe<TestMessage, TestMessageHandler1>();
            _subscriberManager.UnSubscribe<TestDynamicMessageHandler>("TestDynamicMessageHandler");
            _subscriberManager.UnSubscribe<TestDynamicMessageHandler1>("TestDynamicMessageHandler");

            Assert.True(_subscriberManager.IsEmpty);
        }

        [Fact(DisplayName = "获取所有订阅者。")]
        public void Get_Handlers_For_Message_Should_Return_All_Handlers()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler1>();

            var subscribers = _subscriberManager.GetSubscribers<TestMessage>();
            Assert.Equal(2, subscribers.Count());

            _subscriberManager.Subscribe<TestDynamicMessageHandler>("TestDynamicMessageHandler");
            _subscriberManager.Subscribe<TestDynamicMessageHandler1>("TestDynamicMessageHandler");

            var dynamicSubscribers = _subscriberManager.GetSubscribers("TestDynamicMessageHandler");
            Assert.Equal(2, dynamicSubscribers.Count());
        }
    }
}
