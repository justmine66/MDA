using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Transferring;
using MDA.Application.Commands;

namespace EBank.Application
{
    public class EBankApplicationService : IEBankApplicationService
    {
        private readonly IApplicationCommandService _commandService;

        public EBankApplicationService(IApplicationCommandService commandService) => _commandService = commandService;

        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="command"></param>
        public void OpenAccount(OpenBankAccountApplicationCommand command) => _commandService.Publish(command);

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        public void WithdrawFunds(StartWithdrawAccountTransactionApplicationCommand command) => _commandService.Publish(command);

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command"></param>
        public void DepositedFunds(StartDepositAccountTransactionApplicationCommand command) => _commandService.Publish(command);

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command"></param>
        public void TransferFunds(TransferFundsApplicationCommand command) => _commandService.Publish(command);
    }
}
