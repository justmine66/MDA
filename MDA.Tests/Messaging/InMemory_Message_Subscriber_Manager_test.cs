using MDA.Message;
using MDA.Message.Abstractions;
using System.Linq;
using Xunit;

namespace MDA.Tests.Messaging
{
    /// <summary>
    /// 内存订阅管理器测试类。
    /// </summary>
    public class InMemory_Message_Subscriber_Manager_Test
    {
        private readonly IMessageSubscriberManager _subscriberManager = new InMemoryMessageSubscriberManager(new DefaultMessageSubscriberCollection());

        [Fact(DisplayName = "第一次创建，订阅者应该为空。")]
        public void After_Creation_Should_Be_Empty()
        {
            var subscriberManager = new InMemoryMessageSubscriberManager(new DefaultMessageSubscriberCollection());

            Assert.True(subscriberManager.IsEmpty);
        }

        [Fact(DisplayName = "添加一个订阅者后，应该包含该订阅者。")]
        public void After_One_Subscriber_was_Added_Should_Contain_The_Subscriber()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");

            Assert.True(_subscriberManager.HasSubscriber<TestMessage>());
            Assert.True(_subscriberManager.HasSubscriber("TestDynamicMessageHandler"));
        }

        [Fact(DisplayName = "添加一个订阅者后，消息处理者应该唯一。")]
        public void After_One_Subcriber_Was_Added_The_Message_Handler_Should_Be_One()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");

            var subscriblers = _subscriberManager.GetSubscribers<TestMessage>();
            var dynamicSubscriblers = _subscriberManager.GetSubscribers("TestDynamicMessageHandler");

            Assert.Single(subscriblers);
            Assert.Single(dynamicSubscriblers);
        }

        [Fact(DisplayName = "移除所有的订阅者后，订阅者列表应该为空。")]
        public void After_All_Subcriber_Be_Removed_Subscriber_Should_Be_Empty()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler1>();
            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");
            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler1>("TestDynamicMessageHandler");

            _subscriberManager.Unsubscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Unsubscribe<TestMessage, TestMessageHandler1>();
            _subscriberManager.UnsubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");
            _subscriberManager.UnsubscribeDynamic<TestDynamicMessageHandler1>("TestDynamicMessageHandler");

            Assert.True(_subscriberManager.IsEmpty);
        }

        [Fact(DisplayName = "获取所有订阅者。")]
        public void Get_Handlers_For_Message_Should_Return_All_Handlers()
        {
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler>();
            _subscriberManager.Subscribe<TestMessage, TestMessageHandler1>();

            var _subscribers = _subscriberManager.GetSubscribers<TestMessage>();
            Assert.Equal(2, _subscribers.Count());

            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler>("TestDynamicMessageHandler");
            _subscriberManager.SubscribeDynamic<TestDynamicMessageHandler1>("TestDynamicMessageHandler");

            var _dynamicSubscribers = _subscriberManager.GetSubscribers("TestDynamicMessageHandler");
            Assert.Equal(2, _dynamicSubscribers.Count());
        }
    }
}
