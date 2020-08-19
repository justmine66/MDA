using MDA.Application.Commands;

namespace EBank.Application.Commands.Withdrawing
{
    /// <summary>
    /// 确认取款交易信息验证通过的应用层命令
    /// </summary>
    public class ConfirmWithdrawTransactionValidatePassedApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }
    }
}
