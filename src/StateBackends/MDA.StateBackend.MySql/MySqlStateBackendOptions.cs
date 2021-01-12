namespace MDA.StateBackend.MySql
{
    /// <summary>
    /// MySql 状态后端配置项
    /// </summary>
    public class MySqlStateBackendOptions
    {
        /// <summary>
        /// 连接字符串配置项
        /// </summary>
        public ConnectionStringOptions ConnectionStrings { get; set; } = new ConnectionStringOptions();

        /// <summary>
        /// 表名配置项
        /// </summary>
        public TableOptions Tables { get; set; } = new TableOptions();

        /// <summary>
        /// 连接字符串配置项
        /// </summary>
        public class ConnectionStringOptions
        {
            /// <summary>
            /// 查询库，存储视图数据，面向前台设计。
            /// </summary>
            public string ReadDb { get; set; }

            /// <summary>
            /// 状态库，存储系统状态数据，面向后台设计。
            /// </summary>
            public string StateDb { get; set; }
        }

        /// <summary>
        /// 表名配置项
        /// </summary>
        public class TableOptions
        {
            /// <summary>
            /// 领域事件配置项
            /// </summary>
            public DomainEventTables DomainEventOptions { get; set; } = new DomainEventTables();

            /// <summary>
            /// 领域模型配置项
            /// </summary>
            public DomainModelTables DomainModelOptions { get; set; } = new DomainModelTables();

            /// <summary>
            /// 领域事件表列表
            /// </summary>
            public class DomainEventTables
            {
                public string DomainEventIndices { get; set; } = "domain_event_indices";

                public string DomainEvents { get; set; } = "domain_events";
            }

            /// <summary>
            /// 领域模型表列表
            /// </summary>
            public class DomainModelTables
            {
                public string AggregateRootCheckpointIndices { get; set; } = "aggregate_root_checkpoint_indices";

                public string AggregateRootCheckpoints { get; set; } = "aggregate_root_checkpoints";
            }
        }
    }
}
