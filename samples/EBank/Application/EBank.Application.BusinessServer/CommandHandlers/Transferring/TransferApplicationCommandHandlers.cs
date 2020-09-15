using EBank.Application.Commands.Transferring;
using MDA.Application.Commands;

namespace EBank.Application.BusinessServer.CommandHandlers.Transferring
{
    public class TransferApplicationCommandHandlers : 
        IApplicationCommandHandler<TransferFundsApplicationCommand>,
        IApplicationCommandHandler<ConfirmTransferTransactionValidatePassedApplicationCommand>
    {
        public void OnApplicationCommand(IApplicationCommandContext context, TransferFundsApplicationCommand command)
        {
            // 1. 发起转账交易
            context.DomainCommandPublisher.Publish(ConfirmDepositTransactionValidatePassedDomainCommandTranslator.Instance, command);
        }

        public void OnApplicationCommand(IApplicationCommandContext context,
            ConfirmTransferTransactionValidatePassedApplicationCommand command)
        {
            // 2. 验证账户
            // context.DomainCommandPublisher.Publish(ConfirmDepositTransactionValidatePassedDomainCommandTranslator.Instance, command);
        }
    }
}
