using MDA.MessageBus;
using MDA.MessageBus.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace MDA.Tests.Messaging
{
    /// <summary>
    /// 内存消息总线测试类
    /// </summary>
    public class InMemoryMessageBusTest
    {
        private readonly IMessageBus _messageBus;

        public InMemoryMessageBusTest()
        {
            var provider = new ServiceCollection()
                .AddLogging()
                .AddMessageBusBasicServices(options => options.MonitorSlowMessageHandler = true)
                .AddInMemoryMessageBusServices()
                .AddScoped<TestMessageHandler>()
                .AddScoped<TestDynamicMessageHandler>()
                .AddScoped<TestSlowMessageHandler>()
                .BuildServiceProvider();

            _messageBus = provider.GetService<IMessageBus>();
        }

        [Fact(DisplayName = "发布一条类型化消息，消息处理者应该接收到。")]
        public async Task After_Push_One_Typed_Message_Handler_Should_Received()
        {
            _messageBus.Subscribe<TestMessage, TestMessageHandler>();
            await _messageBus.PublishAsync(new TestMessage());

            Assert.True(TestMessageHandler.ReceivedMessage);
        }

        [Fact(DisplayName = "发布一条动态消息，消息处理者应该接收到。")]
        public async Task After_Push_One_Dynamic_Message_Handler_Should_Received()
        {
            _messageBus.Subscribe<TestDynamicMessageHandler>("TestMessage");
            await _messageBus.PublishAsync("TestMessage", new TestMessage());

            Assert.True(TestDynamicMessageHandler.ReceivedMessage);
        }

        [Fact(DisplayName = "监视慢消息")]
        public async Task Monitor_Slow_Message_Handler()
        {
            var isSlowMessage = false;
            if (_messageBus is ISlowMessageAware aware)
            {
                aware.OnSlowMessage += delegate (object sender, SlowMessageEventArgs args)
                {
                    Debugger.Log(2, "OnSlowMessageHandled", args.ToString());

                    isSlowMessage = true;
                };
            }

            _messageBus.Subscribe<TestMessage, TestSlowMessageHandler>();
            await _messageBus.PublishAsync(new TestMessage());

            Assert.True(isSlowMessage);
        }
    }
}
