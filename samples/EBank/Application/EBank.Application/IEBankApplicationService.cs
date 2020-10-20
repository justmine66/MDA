using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Transferring;
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
        Task DepositedFundsAsync(StartDepositAccountTransactionApplicationCommand command, CancellationToken token = default);

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token">取消令牌</param>
        Task WithdrawFundsAsync(StartWithdrawAccountTransactionApplicationCommand command, CancellationToken token = default);

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="token">取消令牌</param>
        Task TransferFundsAsync(TransferFundsApplicationCommand command, CancellationToken token = default);
    }
}
