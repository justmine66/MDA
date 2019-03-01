using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MDA.MessageBus.Kafka.Impl
{
    public class KafkaPersistentConnectorFactory : IKafkaPersistentConnectorFactory
    {
        private readonly IServiceProvider _container;

        public KafkaPersistentConnectorFactory(IServiceProvider container)
        {
            _container = container;
        }

        public IKafkaPersistentConnector Create(ChannelType type)
        {
            var services = _container.GetServices<IKafkaPersistentConnector>();

            switch (type)
            {
                case ChannelType.Producer:
                    return services.OfType<KafkaProducerPersistentConnector>().First();
                case ChannelType.Consumer:
                    return services.OfType<KafkaConsumerPersistentConnector>().First();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
