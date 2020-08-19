using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Transferring;

namespace EBank.Application
{
    /// <summary>
    /// 银行应用逻辑服务
    /// </summary>
    public interface IBankApplicationService
    {
        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="command">命令</param>
        void OpenAccount(OpenBankAccountApplicationCommand command);
        
        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        void WithdrawFunds(StartWithdrawAccountTransactionApplicationCommand command);

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command">命令</param>
        void DepositedFunds(StartDepositAccountTransactionApplicationCommand command);

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command">命令</param>
        void TransferFunds(TransferFundsApplicationCommand command);
    }
}
