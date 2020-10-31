using MDA.Domain.Events;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;

namespace MDA.Domain.DependencyInjection
{
    public static class DomainConfigureContextExtensions
    {
        /// <summary>
        /// 配置入站、出站消息总线。
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="name">消息总线客户端名称</param>
        /// <returns>上下文</returns>
        public static IDomainConfigureContext UseMessageBus(this IDomainConfigureContext context, MessageBusClientNames name)
        {
            context.Services.AddTypedMessagePublisher<IDomainEventPublisher, DefaultDomainEventPublisher>(name);

            return context;
        }
    }
}
