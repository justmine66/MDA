using MDA.Domain.Commands;
using System.Collections.Generic;

namespace MDA.Domain.Exceptions
{
    public static class DomainExceptionExtensions
    {
        public static IEnumerable<IDomainExceptionMessage> FillDomainCommandInfo(this IEnumerable<IDomainExceptionMessage> exceptions, IDomainCommand command)
        {
            foreach (var notification in exceptions)
            {
                yield return FillDomainCommandInfo(notification, command);
            }
        }

        public static IDomainExceptionMessage FillDomainCommandInfo(this IDomainExceptionMessage exception, IDomainCommand command)
        {
            exception.AggregateRootType = command.AggregateRootType.FullName;
            exception.AggregateRootId = command.AggregateRootId;
            exception.DomainCommandId = command.Id;
            exception.DomainCommandType = command.GetType().FullName;
            exception.Topic = command.Topic;
            exception.PartitionKey = command.PartitionKey;
            exception.ApplicationCommandId = command.ApplicationCommandId;
            exception.ApplicationCommandType = command.ApplicationCommandType;

            return exception;
        }
    }
}
