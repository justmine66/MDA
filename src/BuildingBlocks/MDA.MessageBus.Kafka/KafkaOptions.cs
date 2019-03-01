using System.Collections.Generic;

namespace MDA.MessageBus.Kafka
{
    public class KafkaOptions
    {
        /// <summary>
        /// Kafka服务器列表，多个服务以逗号相隔。
        /// </summary>
        public string Servers { get; set; }

        /// <summary>
        /// 生产者配置信息
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> ProducerSettings { get; set; }

        /// <summary>
        /// 消费者配置信息
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> ConsumerSettings { get; set; }
    }
}
