using EBank.Application.Commands.Withdrawing;
using EBank.Domain.Commands.Withdrawing;
using MDA.Application.Commands;
using MDA.Domain.Shared;

namespace EBank.Application.BusinessServer.Processors
{
    /// <summary>
    /// 取款交易应用层命令处理器
    /// </summary>
    public class WithdrawTransactionApplicationCommandProcessor :
        IApplicationCommandHandler<StartWithdrawApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, StartWithdrawApplicationCommand appCommand)
        {
            var transactionId = SnowflakeId.Default().NextId();
            var command = new StartWithdrawTransactionDomainCommand(transactionId, appCommand.AccountId, appCommand.AccountName,
                appCommand.Amount, appCommand.Bank);

            context.DomainCommandPublisher.Publish(command);
        }
    }
}
