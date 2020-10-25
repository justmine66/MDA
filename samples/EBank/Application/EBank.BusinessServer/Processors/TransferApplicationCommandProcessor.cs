using EBank.Application.Commanding.Transferring;
using EBank.Domain.Commands.Transferring;
using EBank.Domain.Models.Transferring;
using MDA.Application.Commands;
using MDA.Shared.Utils;

namespace EBank.BusinessServer.Processors
{
    public class TransferApplicationCommandProcessor :
        IApplicationCommandHandler<StartTransferApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, StartTransferApplicationCommand command)
        {
            var transactionId = SnowflakeId.Default().NextId();
            var source = command.SourceAccount;
            var sink = command.SinkAccount;
            var domainCommand = new StartTransferTransactionDomainCommand(
                transactionId,
                new TransferAccountInfo(source.Id, source.Name, source.Bank),
                new TransferAccountInfo(sink.Id, sink.Name, sink.Bank),
                command.Amount);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
