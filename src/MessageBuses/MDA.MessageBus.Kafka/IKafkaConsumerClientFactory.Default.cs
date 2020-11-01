using Confluent.Kafka;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;

namespace MDA.MessageBus.Kafka
{
    public class DefaultKafkaConsumerClientFactory : IKafkaConsumerClientFactory
    {
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        private readonly KafkaConsumerOptions _options;
        private IConsumer<string, byte[]> _consumerClient;

        private readonly ILogger<DefaultKafkaConsumer> _logger;

        public DefaultKafkaConsumerClientFactory(
            IOptions<KafkaConsumerOptions> options,
            ILogger<DefaultKafkaConsumer> logger)
        {
            _logger = logger;
            _options = options.Value;
        }

        public IConsumer<string, byte[]> CreateClient()
        {
            if (_consumerClient != null)
            {
                return _consumerClient;
            }

            _connectionLock.Wait();

            try
            {
                if (_consumerClient != null)
                    return _consumerClient;

                var config = KafkaUnderlyingConfigBuilder.Singleton
                    .Clear()
                    .Append(_options)
                    .Build();

                _consumerClient = new ConsumerBuilder<string, byte[]>(config)
                    .SetErrorHandler(ConsumerClient_OnConsumeError)
                    .Build();

                var topics = _options.Topics;
                if (topics.IsEmpty())
                {
                    throw new ArgumentException();
                }

                _consumerClient.Subscribe(topics);

                _logger.LogInformation($"Subscribed topics: {topics.Aggregate((x, y) => $"{x},{y}")}");

                return _consumerClient;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private void ConsumerClient_OnConsumeError(IConsumer<string, byte[]> consumer, Error e)
            => _logger.LogError($"An error occurred during connect kafka --> {e.Reason}");
    }
}
