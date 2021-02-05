using MDA.Domain.Commands;

namespace MDA.Domain.Exceptions
{
    public static class DomainExceptionExtensions
    {
        public static IDomainExceptionMessage FillFrom(this IDomainExceptionMessage exception, IDomainCommand command)
        {
            exception.AggregateRootType = command.AggregateRootType.FullName;
            exception.AggregateRootId = command.AggregateRootId;
            exception.DomainCommandId = command.Id;
            exception.DomainCommandType = command.GetType().FullName;
            exception.Topic = command.Topic;
            exception.PartitionKey = command.PartitionKey;
            exception.ApplicationCommandId = command.ApplicationCommandId;
            exception.ApplicationCommandType = command.ApplicationCommandType;
            exception.ApplicationCommandReplyScheme = command.ApplicationCommandReplyScheme;

            return exception;
        }
    }
}
