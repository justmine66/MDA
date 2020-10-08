namespace MDA.StateBackend.MySql
{
    public class MySqlStateBackendOptions
    {
        public string ConnectionString { get; set; }
        public DomainEventOptions DomainEventOptions { get; set; }
    }

    public class DomainEventOptions
    {
        public class Tables
        {
            public string DomainEvents { get; set; } = "domain_events";

            public string DomainEventContents { get; set; } = "domain_event_payloads";
        }
    }
}
