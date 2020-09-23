using MDA.Application.Commands;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class FakeApplicationCommandProcessor : IApplicationCommandHandler<CreateApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, CreateApplicationCommand command)
        {
            var domainCommand = new CreateDomainCommand(command.Payload);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
