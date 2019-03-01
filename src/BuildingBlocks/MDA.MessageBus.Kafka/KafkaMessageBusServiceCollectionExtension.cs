using MDA.MessageBus.Kafka.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.Kafka
{
    public static class KafkaMessageBusServiceCollectionExtension
    {
        public static IServiceCollection AddKafkaMessageBusServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, KafkaMessageBus>();
            services.AddSingleton<IMessagePublisher, KafkaMessageProducer>();
            services.AddSingleton<IMessageSubscriber, KafkaMessageConsumer>();
            services.AddSingleton<IKafkaPersistentConnector, KafkaProducerPersistentConnector>();
            services.AddSingleton<IKafkaPersistentConnector, KafkaConsumerPersistentConnector>();
            services.AddSingleton<IKafkaPersistentConnectorFactory, KafkaPersistentConnectorFactory>();

            return services;
        }
    }
}
