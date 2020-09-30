namespace MDA.StateBackend.MySql
{
    public class MySqlStateBackendOptions
    {
        public DomainEventOptions DomainEventOptions { get; set; }
    }

    public class DomainEventOptions
    {
        public string Table { get; set; } = "mda_domain_events";
    }
}
