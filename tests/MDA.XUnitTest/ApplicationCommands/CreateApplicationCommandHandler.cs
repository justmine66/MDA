using MDA.Application.Commands;
using MDA.Domain.Commands;
using MDA.XUnitTest.BusinessProcessing;
using Microsoft.Extensions.Logging;

namespace MDA.XUnitTest.ApplicationCommands
{
    public class CreateApplicationCommandHandler : IApplicationCommandHandler<CreateApplicationCommand>
    {
        private readonly ILogger _logger;
        private readonly IDomainCommandPublisher _domainCommandPublisher;

        public CreateApplicationCommandHandler(
            ILogger<CreateApplicationCommandHandler> logger,
            IDomainCommandPublisher domainCommandPublisher)
        {
            _logger = logger;
            _domainCommandPublisher = domainCommandPublisher;
        }

        public void OnApplicationCommand(IApplicationCommandContext context, CreateApplicationCommand command)
        {
            _logger.LogInformation($"The application notification: {nameof(command)}[Payload: {command.Payload}] handled.");

            var create = new CreateDomainCommand(command.Payload);

            _domainCommandPublisher.Publish(create);
        }
    }
}
