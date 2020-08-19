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
        public void Handle(IApplicationCommandContext context, ConfirmWithdrawTransactionValidatePassedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmWithdrawTransactionValidatePassedDomainCommandTranslator.Instance);
        }

        public void Handle(IApplicationCommandContext context, CancelWithdrawTransactionApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(CancelWithdrawTransactionDomainCommandTranslator.Instance);
        }

        public void Handle(IApplicationCommandContext context, ConfirmWithdrawTransactionCompletedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmWithdrawTransactionCompletedDomainCommandTranslator.Instance);
        }
    }
}
