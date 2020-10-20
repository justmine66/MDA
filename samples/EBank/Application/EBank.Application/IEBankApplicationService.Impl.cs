using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Transferring;
using MDA.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="token"></param>
        public async Task OpenAccountAsync(OpenBankAccountApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task WithdrawFundsAsync(StartWithdrawAccountTransactionApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }



        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task DepositedFundsAsync(StartDepositAccountTransactionApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task TransferFundsAsync(TransferFundsApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }
    }
}
