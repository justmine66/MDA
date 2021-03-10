using MDA.Domain.Commands;

namespace MDA.Application.Commands
{
    public static class ApplicationCommandingExtensions
    {
        public static IDomainCommand FillFrom(this IDomainCommand domainCommand, IApplicationCommand applicationCommand)
        {
            domainCommand.ApplicationCommandId = applicationCommand.Id;
            domainCommand.ApplicationCommandType = applicationCommand.GetType().FullName;
            domainCommand.ApplicationCommandReplyScheme = applicationCommand.ReplyScheme;

            return domainCommand;
        }

        public static IDomainCommand<TAggregateRootId> FillFrom<TAggregateRootId>(this IDomainCommand<TAggregateRootId> domainCommand, IApplicationCommand applicationCommand)
        {
            domainCommand.ApplicationCommandId = applicationCommand.Id;
            domainCommand.ApplicationCommandType = applicationCommand.GetType().FullName;
            domainCommand.ApplicationCommandReplyScheme = applicationCommand.ReplyScheme;

            return domainCommand;
        }
    }
}
