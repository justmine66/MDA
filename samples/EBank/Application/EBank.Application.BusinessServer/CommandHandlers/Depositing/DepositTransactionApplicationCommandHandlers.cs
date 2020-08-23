using EBank.Application.Commands.Depositing;
using MDA.Application.Commands;

namespace EBank.Application.BusinessServer.CommandHandlers.Depositing
{
    /// <summary>
    /// 存款交易应用层命令处理器
    /// </summary>
    public class DepositTransactionApplicationCommandHandlers :
        IApplicationCommandHandler<ConfirmDepositTransactionValidatePassedApplicationCommand>,
        IApplicationCommandHandler<CancelDepositTransactionApplicationCommand>,
        IApplicationCommandHandler<ConfirmDepositTransactionCompletedApplicationCommand>
    {
        public void Handle(IApplicationCommandContext context, ConfirmDepositTransactionValidatePassedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmDepositTransactionValidatePassedDomainCommandTranslator.Instance, command);
        }

        public void Handle(IApplicationCommandContext context, CancelDepositTransactionApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(CancelDepositTransactionDomainCommandTranslator.Instance, command);
        }

        public void Handle(IApplicationCommandContext context, ConfirmDepositTransactionCompletedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmDepositTransactionCompletedDomainCommandTranslator.Instance, command);
        }
    }
}
