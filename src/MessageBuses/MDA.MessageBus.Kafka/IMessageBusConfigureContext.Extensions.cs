using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus.Kafka
{
    public static class MessageBusConfigureContextExtensions
    {
        public static IMessageBusConfigureContext AddKafka(this IMessageBusConfigureContext context, IConfiguration configuration)
        {
            var kafkaOptions = configuration.GetSection("MessageBuses").GetSection("Kafka");
            var producerOptions = kafkaOptions.GetSection("ProducerOptions");
            var consumerOptions = kafkaOptions.GetSection("ConsumerOptions");

            context.Services.Configure<KafkaOptions>(kafkaOptions);
            context.Services.Configure<KafkaProducerOptions>(producerOptions);
            context.Services.Configure<KafkaConsumerOptions>(consumerOptions);

            context.Services.AddSingleton<IKafkaProducerPool, DefaultKafkaProducerPool>();
            context.Services.AddSingleton<IKafkaConsumerClientFactory, DefaultKafkaConsumerClientFactory>();
            context.Services.AddSingleton<IKafkaConsumer, DefaultKafkaConsumer>();
            context.Services.AddSingleton<IMessageQueueService, KafkaMessageQueueService>();
            context.Services.AddSingleton<IAsyncMessageQueueService, KafkaAsyncMessageQueueService>();

            return context;
        }
    }
}