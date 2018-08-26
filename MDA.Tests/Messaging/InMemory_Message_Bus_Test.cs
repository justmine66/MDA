using MDA.Messaging;
using MDA.Messaging.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace MDA.Tests.Messaging
{
    /// <summary>
    /// 内存消息总线测试类
    /// </summary>
    public class InMemory_Message_Bus_Test
    {
        private readonly IMessageBus _messageBus;

        public InMemory_Message_Bus_Test()
        {
            var provider = new ServiceCollection()
                .AddLogging()
                .AddMessaging(IsReigisterInMemoryBus: true)
                .AddScoped<TestMessageHandler>()
                .AddScoped<TestDynamicMessageHandler>()
                .BuildServiceProvider();

            _messageBus = provider.GetService<IMessageBus>();
        }

        [Fact(DisplayName = "发布一条类型化消息，消息处理者应该接收到。")]
        public async Task After_Push_One_Typed_Message_Handler_Should_Recevied()
        {
            _messageBus.Subscribe<TestMessage, TestMessageHandler>();
            await _messageBus.PublishAsync(new TestMessage());

            Assert.True(TestMessageHandler.ReceivedMessage);
        }

        [Fact(DisplayName = "发布一条动态消息，消息处理者应该接收到。")]
        public async Task After_Push_One_Dynamic_Message_Handler_Should_Recevied()
        {
            _messageBus.SubscribeDynamic<TestDynamicMessageHandler>("TestMessage");
            await _messageBus.PublishDynamicAsync("TestMessage", new TestMessage());

            Assert.True(TestDynamicMessageHandler.ReceivedMessage);
        }
    }
}
