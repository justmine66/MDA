using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MDA.MessageBus.Kafka
{
    public class KafkaConsumer
    {
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        private readonly string _groupId;
        private readonly KafkaOptions _kafkaOptions;
        private IConsumer<string, byte[]> _consumerClient;

        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(string groupId, 
            IOptions<KafkaOptions> options,
            ILogger<KafkaConsumer> logger)
        {
            _groupId = groupId;
            _logger = logger;
            _kafkaOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            Connect();

            _consumerClient.Subscribe(topics);
        }

        private void Connect()
        {
            if (_consumerClient != null)
            {
                return;
            }

            _connectionLock.Wait();

            try
            {
                if (_consumerClient != null) return;

                _kafkaOptions.MainConfig["group.id"] = _groupId;
                _kafkaOptions.MainConfig["auto.offset.reset"] = "earliest";

                var config = _kafkaOptions.AsKafkaConfig();

                _consumerClient = new ConsumerBuilder<string, byte[]>(config)
                    .SetErrorHandler(ConsumerClient_OnConsumeError)
                    .Build();
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
