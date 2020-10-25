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
            _kafkaOptions = options.Value;
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

                // 一、设置消费者组
                _kafkaOptions.MainConfig["group.id"] = _groupId;

                // 二、设置初始消费偏移量
                // 1. earliest: 从分区最早的偏移量开始消费
                // 2. earliest: 从分区最新的偏移量开始消费
                // 3. none: 未找到偏移量则抛出异常
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
