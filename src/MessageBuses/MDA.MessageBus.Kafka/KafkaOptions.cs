namespace MDA.MessageBus.Kafka
{
    public abstract class KafkaOptions
    {
        /// <summary>
        /// Broker 服务列表，示例：192.168.2.112:9092,192.168.2.113:9092。
        /// </summary>
        public string BrokerServers { get; set; }
    }
}
