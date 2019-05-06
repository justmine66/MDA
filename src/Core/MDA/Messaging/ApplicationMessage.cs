namespace MDA.Messaging
{
    public abstract class ApplicationMessage : IApplicationMessage
    {
        public string BusinessPrincipalId { get; set; }
        public string BusinessPrincipalTypeName { get; set; }
        public string CommandId { get; set; }
        public string CommandTime { get; set; }
        public string CommandProcessingTime { get; set; }
        public string DomainEventId { get; set; }
        public string DomainEventTime { get; set; }
        public string DomainEventProcessingTime { get; set; }
    }
}
