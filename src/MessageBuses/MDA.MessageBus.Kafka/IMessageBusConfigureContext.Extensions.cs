using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.Kafka
{
    public static class MessageBusConfigureContextExtensions
    {
        public static IMessageBusConfigureContext UseKafka(this IMessageBusConfigureContext context, IConfiguration configuration)
        {
            context.Services.Configure<KafkaProducerOptions>(configuration.GetSection(nameof(KafkaProducerOptions)));
            context.Services.Configure<KafkaConsumerOptions>(configuration.GetSection(nameof(KafkaConsumerOptions)));
            context.Services.AddSingleton<IKafkaProducerPool, DefaultKafkaProducerPool>();
            context.Services.AddSingleton<IKafkaConsumerClientFactory, DefaultKafkaConsumerClientFactory>();
            context.Services.AddSingleton<IKafkaConsumer, DefaultKafkaConsumer>();
            context.Services.AddSingleton<IMessageQueueService, KafkaMessageQueueService>();
            context.Services.AddSingleton<IAsyncMessageQueueService, KafkaAsyncMessageQueueService>();

            return context;
        }
    }
}
