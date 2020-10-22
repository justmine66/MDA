using EBank.Application.Commands.Depositing;
using EBank.Domain.Commands.Depositing;
using MDA.Application.Commands;
using MDA.Domain.Shared;

namespace EBank.Application.BusinessServer.Processors
{
    /// <summary>
    /// 存款交易应用层命令处理器
    /// </summary>
    public class DepositTransactionApplicationCommandProcessor :
        IApplicationCommandHandler<StartDepositApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, StartDepositApplicationCommand appCommand)
        {
            var transactionId = SnowflakeId.Default().NextId();
            var domainCommand = new StartDepositTransactionDomainCommand(transactionId, appCommand.AccountId, appCommand.AccountName,
                appCommand.Amount, appCommand.Bank);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
