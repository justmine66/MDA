﻿using MDA.Infrastructure.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MDA.MessageBus.Kafka
{
    public class KafkaUnderlyingConfigBuilder
    {
        private static readonly Lazy<KafkaUnderlyingConfigBuilder> Instance = new Lazy<KafkaUnderlyingConfigBuilder>(() => new KafkaUnderlyingConfigBuilder());

        public static KafkaUnderlyingConfigBuilder Singleton = Instance.Value;

        /// <summary>
        /// refer to https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md.
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _kafkaConfig = new ConcurrentDictionary<string, string>();

        public KafkaUnderlyingConfigBuilder Append(string key, string value)
        {
            _kafkaConfig[key] = value;

            return this;
        }

        public KafkaUnderlyingConfigBuilder Append(string key, long value)
        {
            _kafkaConfig[key] = value.ToString();

            return this;
        }

        public KafkaUnderlyingConfigBuilder Append(string key, bool value)
        {
            _kafkaConfig[key] = value.ToString();

            return this;
        }

        public KafkaUnderlyingConfigBuilder Append(KafkaProducerOptions options)
        {
            if (options.BrokerServers == null || options.BrokerServers.Length <= 0)
            {
                throw new ArgumentNullException(nameof(options.BrokerServers));
            }

            Append("bootstrap.servers", options.BrokerServers.Aggregate((x, y) => $"{x},{y}"));
            Append("request.required.acks", options.AcknowledgeOptions.Type);
            Append("request.timeout.ms", options.AcknowledgeOptions.TimeoutMilliseconds);
            Append("message.send.max.retries", options.AcknowledgeOptions.MaxRetries);
            Append("enable.idempotence", options.IdempotentOptions.Enable);

            return this;
        }

        public KafkaUnderlyingConfigBuilder Append(KafkaConsumerOptions options)
        {
            if (options.BrokerServers == null || options.BrokerServers.Length <= 0)
            {
                throw new ArgumentNullException(nameof(options.BrokerServers));
            }
            if (options.Topics == null || options.Topics.Length <= 0)
            {
                throw new ArgumentNullException(nameof(options.BrokerServers));
            }
            PreConditions.NotNullOrEmpty(options.Group, nameof(options.Group));

            Append("bootstrap.servers", options.BrokerServers.Aggregate((x, y) => $"{x},{y}"));
            Append("allow.auto.create.topics", options.AllowAutoCreateTopic);
            Append("group.id", options.Group);
            Append("auto.offset.reset", options.BootstrapOffsetOptions.AutoResetType);
            Append("enable.auto.commit", options.AutoCommitOptions.Enable);
            Append("auto.commit.interval.ms", options.AutoCommitOptions.IntervalMilliseconds);

            return this;
        }

        public IEnumerable<KeyValuePair<string, string>> Build() => _kafkaConfig;

        public KafkaUnderlyingConfigBuilder Clear()
        {
            _kafkaConfig.Clear();

            return this;
        }
    }
}
