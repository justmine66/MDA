using EBank.Application.Commanding.Depositing;
using EBank.Domain.Commands.Depositing;
using MDA.Application.Commands;
using MDA.Infrastructure.Utils;

namespace EBank.DomainServer.Processors
{
    /// <summary>
    /// 存款交易应用层命令处理器
    /// </summary>
    public class DepositTransactionApplicationCommandProcessor :
        IApplicationCommandHandler<StartDepositApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandingContext context, StartDepositApplicationCommand appCommand)
        {
            var transactionId = SnowflakeId.Default().NextId();
            var domainCommand = new StartDepositTransactionDomainCommand(transactionId, appCommand.AccountId,
                appCommand.AccountName,
                appCommand.Amount, appCommand.Bank)
            {
                ApplicationCommandId = appCommand.Id,
                ApplicationCommandType = appCommand.GetType().FullName,
                ApplicationCommandReplyScheme = appCommand.ReplyScheme
            };

            context.PublishDomainCommand(domainCommand);
        }
    }
}
