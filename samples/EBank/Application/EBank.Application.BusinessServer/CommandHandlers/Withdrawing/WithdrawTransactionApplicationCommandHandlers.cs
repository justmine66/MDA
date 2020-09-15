using EBank.Application.Commands.Withdrawing;
using MDA.Application.Commands;

namespace EBank.Application.BusinessServer.CommandHandlers.Withdrawing
{
    /// <summary>
    /// 取款交易应用层命令处理器
    /// </summary>
    public class WithdrawTransactionApplicationCommandHandlers :
        IApplicationCommandHandler<ConfirmWithdrawTransactionValidatePassedApplicationCommand>,
        IApplicationCommandHandler<CancelWithdrawTransactionApplicationCommand>,
        IApplicationCommandHandler<ConfirmWithdrawTransactionCompletedApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, ConfirmWithdrawTransactionValidatePassedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator.Instance, command);
        }

        public void OnApplicationCommand(IApplicationCommandContext context, CancelWithdrawTransactionApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(CancelWithdrawTransactionDomainCommandTranslator.Instance, command);
        }

        public void OnApplicationCommand(IApplicationCommandContext context, ConfirmWithdrawTransactionCompletedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmWithdrawTransactionCompletedDomainCommandTranslator.Instance, command);
        }
    }
}
