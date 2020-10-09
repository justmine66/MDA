using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Transferring;

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
        void OpenAccount(OpenBankAccountApplicationCommand command);

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command">命令</param>
        void DepositedFunds(StartDepositAccountTransactionApplicationCommand command);

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        void WithdrawFunds(StartWithdrawAccountTransactionApplicationCommand command);

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command">命令</param>
        void TransferFunds(TransferFundsApplicationCommand command);
    }
}
