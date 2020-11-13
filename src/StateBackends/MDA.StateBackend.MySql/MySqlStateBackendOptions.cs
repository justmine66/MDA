namespace MDA.StateBackend.MySql
{
    public class MySqlStateBackendOptions
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public DomainEventOptions DomainEventOptions { get; set; } = new DomainEventOptions();
        public DomainModelOptions DomainModelOptions { get; set; } = new DomainModelOptions();
    }

    public class ConnectionStrings
    {
        public string ReadDb { get; set; }

        public string StateDb { get; set; }
    }

    public class DomainEventOptions
    {
        public DomainEventTables Tables { get; set; } = new DomainEventTables();

        public class DomainEventTables
        {
            public string DomainEventIndices { get; set; } = "domain_event_indices";

            public string DomainEvents { get; set; } = "domain_events";
        }
    }

    public class DomainModelOptions
    {
        public DomainModelTables Tables { get; set; } = new DomainModelTables();

        public class DomainModelTables
        {
            public string AggregateRootCheckpointIndices { get; set; } = "aggregate_root_checkpoint_indices";

            public string AggregateRootCheckpoints { get; set; } = "aggregate_root_checkpoints";
        }
    }
}
