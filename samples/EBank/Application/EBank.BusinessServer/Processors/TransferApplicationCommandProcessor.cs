using EBank.Application.Commanding.Transferring;
using EBank.Domain.Commands.Transferring;
using EBank.Domain.Models.Transferring;
using MDA.Application.Commands;
using MDA.Infrastructure.Utils;

namespace EBank.BusinessServer.Processors
{
    public class TransferApplicationCommandProcessor : IApplicationCommandHandler<StartTransferApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, StartTransferApplicationCommand command)
        {
            var transactionId = SnowflakeId.Default().NextId();
            var source = command.SourceAccount;
            var sink = command.SinkAccount;
            var domainCommand = new StartTransferTransactionDomainCommand(
                transactionId,
                new TransferAccount(source.Id, source.Name, source.Bank, TransferAccountType.Source),
                new TransferAccount(sink.Id, sink.Name, sink.Bank, TransferAccountType.Sink),
                command.Amount);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
