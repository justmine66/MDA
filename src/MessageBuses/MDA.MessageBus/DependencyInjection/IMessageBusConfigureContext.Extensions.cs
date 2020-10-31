using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDA.MessageBus.DependencyInjection
{
    public static class MessageBusConfigureContextExtensions
    {
        public static IServiceCollection AddTypedMessagePublisher<TMessagePublisher, TMessagePublisherImpl>(this IServiceCollection services, MessageBusClientNames name)
            where TMessagePublisher : class
            where TMessagePublisherImpl : TMessagePublisher
        {
            services.AddOptions();

            services.TryAddSingleton<IMessagePublisherFactory, DefaultMessagePublisherFactory>();

            services.TryAdd(ServiceDescriptor.Singleton(typeof(ITypedMessagePublisherFactory<>), typeof(DefaultTypedMessagePublisherFactory<>)));

            services.AddTransient<TMessagePublisher>(provider =>
            {
                var messagePublisherFactory = provider.GetRequiredService<IMessagePublisherFactory>();
                var messagePublisher = messagePublisherFactory.CreateMessagePublisher(name);

                var typeMessagePublisherFactory = provider.GetRequiredService<ITypedMessagePublisherFactory<TMessagePublisherImpl>>();

                return typeMessagePublisherFactory.CreateMessagePublisher(messagePublisher);
            });

            return services;
        }

    }
}
