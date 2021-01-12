namespace MDA.MessageBus.Kafka
{
    /// <summary>
    /// Kafka 消费者配置项
    /// </summary>
    public class KafkaConsumerOptions : KafkaOptions
    {
        /// <summary>
        /// 消费组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 主题列表
        /// </summary>
        public string[] Topics { get; set; }

        /// <summary>
        /// 允许自动创建主题，
        /// </summary>
        public bool AllowAutoCreateTopic { get; set; } = true;

        /// <summary>
        /// 初始消费偏移量配置项
        /// </summary>
        public BootstrapOffsetOptions BootstrapOffsetOptions { get; set; } = new BootstrapOffsetOptions();

        /// <summary>
        /// 自动提交策略配置项
        /// </summary>
        public AutoCommitOptions AutoCommitOptions { get; set; } = new AutoCommitOptions();
    }

    /// <summary>
    /// 初始消费偏移量配置项
    /// </summary>
    public class BootstrapOffsetOptions
    {
        /// <summary>
        /// 自动重置类型
        /// </summary>
        public string AutoResetType { get; set; } = BootstrapOffsetAutoResetType.Latest;
    }

    /// <summary>
    /// 初始消费偏移量重置类型
    /// </summary>
    public static class BootstrapOffsetAutoResetType
    {
        /// <summary>
        /// 从分区最早的偏移量开始消费
        /// </summary>
        public const string Earliest = "earliest";

        /// <summary>
        /// 从分区最新的偏移量开始消费
        /// </summary>
        public const string Latest = "latest";

        /// <summary>
        /// 由Kafka内部处理，未找到偏移量则抛出异常。
        /// </summary>
        public const string None = "none";
    }

    /// <summary>
    /// 自动提交策略配置项
    /// </summary>
    public class AutoCommitOptions
    {
        /// <summary>
        /// 启动
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 自动提交间隔
        /// </summary>
        public long IntervalMilliseconds { get; set; } = 5000;
    }
}
