namespace MDA.StateBackend.MySql
{
    public class MySqlStateBackendOptions
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public DomainEventOptions DomainEventOptions { get; set; }
    }

    public class ConnectionStrings
    {
        public string ReadDb { get; set; }

        public string StateDb { get; set; }
    }

    public class DomainEventOptions
    {
        public DomainEventTables Tables { get; set; }

        public class DomainEventTables
        {
            public string DomainEventsIndices { get; set; } = "domain_event_indices";

            public string DomainEventPayloads { get; set; } = "domain_event_payloads";
        }
    }
}
