using EBank.Application.Commands.Depositing;
using MDA.Application.Commands;

namespace EBank.Application.BusinessServer.Processors.Depositing
{
    /// <summary>
    /// 存款交易应用层命令处理器
    /// </summary>
    public class DepositTransactionApplicationCommandProcessor :
        IApplicationCommandHandler<ConfirmDepositTransactionValidatePassedApplicationCommand>,
        IApplicationCommandHandler<CancelDepositTransactionApplicationCommand>,
        IApplicationCommandHandler<ConfirmDepositTransactionCompletedApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, ConfirmDepositTransactionValidatePassedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmDepositTransactionValidatePassedDomainCommandTranslator.Instance, command);
        }

        public void OnApplicationCommand(IApplicationCommandContext context, CancelDepositTransactionApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(CancelDepositTransactionDomainCommandTranslator.Instance, command);
        }

        public void OnApplicationCommand(IApplicationCommandContext context, ConfirmDepositTransactionCompletedApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(ConfirmDepositTransactionCompletedDomainCommandTranslator.Instance, command);
        }
    }
}
