using EBank.Application.Commands.Transferring;
using EBank.Domain.Commands.Transferring;
using MDA.Application.Commands;
using MDA.Domain.Shared;

namespace EBank.Application.BusinessServer.Processors
{
    public class TransferApplicationCommandProcessor :
        IApplicationCommandHandler<StartTransferApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, StartTransferApplicationCommand command)
        {
            var transactionId = SnowflakeId.Default().NextId();
            var domainCommand = new StartTransferTransactionDomainCommand(transactionId, command.SourceAccount, command.SinkAccount, command.Amount);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
