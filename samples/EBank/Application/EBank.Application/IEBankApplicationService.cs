using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Transferring;
using EBank.Application.Commands.Withdrawing;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application
{
    /// <summary>
    /// 电子银行应用层服务
    /// </summary>
    public interface IEBankApplicationService
    {
        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="token">取消令牌</param>
        Task OpenAccountAsync(OpenBankAccountApplicationCommand command, CancellationToken token = default);

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="token">取消令牌</param>
        Task DepositedFundsAsync(StartDepositApplicationCommand command, CancellationToken token = default);

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token">取消令牌</param>
        Task WithdrawFundsAsync(StartWithdrawApplicationCommand command, CancellationToken token = default);

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="token">取消令牌</param>
        Task TransferFundsAsync(StartTransferApplicationCommand command, CancellationToken token = default);
    }
}
