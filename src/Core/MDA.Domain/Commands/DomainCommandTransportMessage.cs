using MDA.MessageBus;

namespace MDA.Domain.Commands
{
    public class DomainCommandTransportMessage : Message
    {
        public DomainCommandTransportMessage(IDomainCommand domainCommand)
        {
            DomainCommand = domainCommand;
            Topic = domainCommand.GetTopic();
            PartitionKey = domainCommand.PartitionKey;
        }

        public IDomainCommand DomainCommand { get; private set; }
    }
}
