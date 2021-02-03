using MDA.Domain.Events;
using MDA.Domain.Exceptions;
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
            var services = context.Services;

            services.AddTypedMessagePublisher<IDomainEventPublisher, DefaultDomainEventPublisher>(name);
            services.AddTypedMessagePublisher<IDomainNotificationPublisher, DefaultDomainNotificationPublisher>(name);
            services.AddTypedMessagePublisher<IDomainExceptionMessagePublisher, DefaultDomainExceptionMessagePublisher>(name);

            services.Configure<DomainEventOptions>(_ => { });
            services.Configure<DomainNotificationOptions>(_ => { });
            services.Configure<DomainExceptionOptions>(_ => { });

            if (configuration == null) return context;

            var domainOptions = configuration.GetSection("MDA").GetSection("DomainOptions");

            services.Configure<DomainEventOptions>(domainOptions.GetSection("EventOptions"));
            services.Configure<DomainNotificationOptions>(domainOptions.GetSection("NotificationOptions"));
            services.Configure<DomainExceptionOptions>(domainOptions.GetSection("ExceptionOptions"));

            return context;
        }
    }
}
