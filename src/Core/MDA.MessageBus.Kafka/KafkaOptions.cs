using Confluent.Kafka;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MDA.MessageBus.Kafka
{
    /// <summary>
    /// Provides programmatic configuration for the CAP kafka project.
    /// </summary>
    public class KafkaOptions
    {
        /// <summary>
        /// librdkafka configuration parameters (refer to https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md).
        /// <para>
        /// Topic configuration parameters are specified via the "default.topic.config" sub-dictionary config parameter.
        /// </para>
        /// </summary>
        public readonly ConcurrentDictionary<string, string> MainConfig;

        private IEnumerable<KeyValuePair<string, string>> _kafkaConfig;

        public KafkaOptions() => MainConfig = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Producer connection pool size, default is 10
        /// </summary>
        public int ConnectionPoolSize { get; set; } = 10;

        /// <summary>
        /// The `bootstrap.servers` item config of <see cref="MainConfig" />.
        /// <para>
        /// Initial list of brokers as a CSV list of broker host or host:port.
        /// </para>
        /// </summary>
        public string Servers { get; set; }

        /// <summary>
        /// If you need to get offset and partition and so on.
        /// </summary>
        public Func<ConsumeResult<string, byte[]>, List<KeyValuePair<string, string>>> CustomHeaders { get; set; }

        internal IEnumerable<KeyValuePair<string, string>> AsKafkaConfig()
        {
            if (_kafkaConfig == null)
            {
                if (string.IsNullOrWhiteSpace(Servers))
                {
                    throw new ArgumentNullException(nameof(Servers));
                }

                // 一、设置消息代理服务列表，参考示例：192.168.2.112:9092,192.168.2.113:9092
                MainConfig["bootstrap.servers"] = Servers;

                // 设置偏移量提交策略
                // 1. true: 由kafka管理偏移量，系统会以auto.commit.interval.ms为间隔定期提交偏移量。
                // 2. false: 由用户管理偏移量。
                MainConfig["enable.auto.commit"] = "true";
                // auto.commit.interval.ms默认值为: 5000。
                MainConfig["auto.commit.interval.ms"] = "5000";

                MainConfig["queue.buffering.max.ms"] = "10";
                MainConfig["allow.auto.create.topics"] = "true";
                MainConfig["log.connection.close"] = "false";

                
                
                MainConfig["message.timeout.ms"] = "5000";

                _kafkaConfig = MainConfig.AsEnumerable();
            }

            return _kafkaConfig;
        }
    }
}
