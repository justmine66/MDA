using EBank.Application.Commanding.Accounts;
using EBank.Application.Commanding.Depositing;
using EBank.Application.Commanding.Transferring;
using EBank.Application.Commanding.Withdrawing;
using MDA.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Business
{
    public class DefaultEBankApplicationService : IEBankApplicationService
    {
        private readonly IApplicationCommandService _commandService;

        public DefaultEBankApplicationService(IApplicationCommandService commandService) 
                => _commandService = commandService;

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
        /// 存款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task DepositedFundsAsync(StartDepositApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task WithdrawFundsAsync(StartWithdrawApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task TransferFundsAsync(StartTransferApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }
    }
}
