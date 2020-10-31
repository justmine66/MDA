namespace MDA.MessageBus.Kafka
{
    /// <summary>
    /// Kafka 生产者配置项
    /// </summary>
    public class KafkaProducerOptions : KafkaOptions
    {
        /// <summary>
        /// Producer connection pool size, default is 10
        /// </summary>
        public int ConnectionPoolSize { get; set; } = 10;

        /// <summary>
        /// 应答配置项
        /// </summary>
        public AcknowledgeOptions AcknowledgeOptions { get; set; } = new AcknowledgeOptions();

        /// <summary>
        /// 幂等配置项
        /// </summary>
        public IdempotentOptions IdempotentOptions { get; set; } = new IdempotentOptions();
    }

    /// <summary>
    /// 幂等配置项
    /// </summary>
    public class IdempotentOptions
    {
        public bool Enable { get; set; } = false;
    }

    /// <summary>
    /// 应答配置项
    /// </summary>
    public class AcknowledgeOptions
    {
        /// <summary>
        /// 类型，请参考 <see cref="AcknowledgeType"/>。
        /// </summary>
        public string Type { get; set; } = AcknowledgeType.Default;

        /// <summary>
        /// 额定应答时长
        /// </summary>
        public long TimeoutMilliseconds { get; set; } = 3000;

        /// <summary>
        /// 最大重试次数，即超过额定应答时长，生产者尝试发送消息的次数。
        /// </summary>
        public int MaxRetries { get; set; } = 2;
    }

    public static class AcknowledgeType
    {
        /// <summary>
        /// 无应答，即Broker不会向生产者发出的消息已收到的确认，故生成者不会重发，无法保证Broker一定收到了消息，这种情况下，生产者只管发，故吞吐最高，一般用于记录审计日志场景。
        /// </summary>
        public const string None = "0";

        /// <summary>
        /// Leader将消息写到本地日志后，向生产者应答，此为默认配置。
        /// 当采用集群模式部署Broker时，在Header记录消息到本地日志后，在Follower复制日志前，如果Header发生故障，则消息可能会丢，但在大多数情况下，消息都是可靠，吞吐较高。
        /// </summary>
        public const string Default = "1";

        /// <summary>
        /// Leader将消息同步到所有的Follower后，向生产者应答，消息不会丢失，吞吐较低，一般用于交易场景。
        /// </summary>
        public const string All = "all";
    }
}
