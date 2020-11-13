using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.DependencyInjection
{
    public static class DomainConfigureContextExtensions
    {
        /// <summary>
        /// 配置入站、出站消息总线。
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="name">消息总线客户端名称</param>
        /// <param name="configuration">配置信息</param>
        /// <returns>上下文</returns>
        public static IDomainConfigureContext UseMessageBus(
            this IDomainConfigureContext context, 
            MessageBusClientNames name, 
            IConfiguration configuration = null)
        {
            context.Services.AddTypedMessagePublisher<IDomainEventPublisher, DefaultDomainEventPublisher>(name);
            context.Services.AddTypedMessagePublisher<IDomainNotificationPublisher, DefaultDomainNotificationPublisher>(name);

            context.Services.Configure<DomainEventOptions>(_ => { });
            if (configuration != null)
            {
                context.Services.Configure<DomainEventOptions>(configuration.GetSection(nameof(DomainEventOptions)));
            }

            context.Services.Configure<DomainNotificationOptions>(_ => { });
            if (configuration != null)
            {
                context.Services.Configure<DomainNotificationOptions>(configuration.GetSection(nameof(DomainNotificationOptions)));
            }

            return context;
        }
    }
}
