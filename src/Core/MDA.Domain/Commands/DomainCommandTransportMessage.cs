using MDA.Domain.Shared.Commands;
using MDA.MessageBus;

namespace MDA.Domain.Commands
{
    public class DomainCommandTransportMessage : Message
    {
        public DomainCommandTransportMessage(IDomainCommand domainCommand)
        {
            DomainCommand = domainCommand;
            Topic = domainCommand.Topic;
            PartitionKey = domainCommand.PartitionKey;
        }

        public DomainCommandTransportMessage(string applicationCommandId, string applicationCommandType,
            IDomainCommand domainCommand)
        {
            ApplicationCommandId = applicationCommandId;
            ApplicationCommandType = applicationCommandType;
            DomainCommand = domainCommand;
            Topic = domainCommand.Topic;
            PartitionKey = domainCommand.PartitionKey;
        }

        public string ApplicationCommandId { get; set; }

        public string ApplicationCommandType { get; set; }

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

        public DomainCommandTransportMessage(string applicationCommandId, string applicationCommandType,
            IDomainCommand<TAggregateRootId> domainCommand)
            : base(applicationCommandId, applicationCommandType, domainCommand)
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

        public DomainCommandTransportMessage(string applicationCommandId, string applicationCommandType,
            IDomainCommand<TId, TAggregateRootId> domainCommand)
            : base(applicationCommandId, applicationCommandType, domainCommand)
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

        public DomainCommandTransportMessage(string applicationCommandId, string applicationCommandType,
            IDomainCommand<TApplicationCommandId, TId, TAggregateRootId> domainCommand)
            : base(applicationCommandId, applicationCommandType, domainCommand)
        {
            DomainCommand = domainCommand;
        }

        public new IDomainCommand<TApplicationCommandId, TId, TAggregateRootId> DomainCommand { get; }
    }
}
