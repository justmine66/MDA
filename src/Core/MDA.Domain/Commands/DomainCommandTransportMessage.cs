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

        public IDomainCommand DomainCommand { get; }
    }

    public class DomainCommandTransportMessage<TAggregateRootId>
        : DomainCommandTransportMessage
    {
        public DomainCommandTransportMessage(IDomainCommand<TAggregateRootId> domainCommand)
            : base(domainCommand)
        {
            DomainCommand = domainCommand;
        }

        public new IDomainCommand<TAggregateRootId> DomainCommand { get; }
    }

    public class DomainCommandTransportMessage<TId, TAggregateRootId>
        : DomainCommandTransportMessage<TAggregateRootId>
    {
        public DomainCommandTransportMessage(IDomainCommand<TId, TAggregateRootId> domainCommand)
            : base(domainCommand)
        {
            DomainCommand = domainCommand;
        }

        public new IDomainCommand<TId, TAggregateRootId> DomainCommand { get; }
    }

    public class DomainCommandTransportMessage<TApplicationCommandId, TId, TAggregateRootId>
        : DomainCommandTransportMessage<TAggregateRootId>
    {
        public DomainCommandTransportMessage(IDomainCommand<TApplicationCommandId, TId, TAggregateRootId> domainCommand)
            : base(domainCommand)
        {
            DomainCommand = domainCommand;
        }

        public new IDomainCommand<TApplicationCommandId, TId, TAggregateRootId> DomainCommand { get; }
    }
}
