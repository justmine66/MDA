using MDA.Application.Commands;

namespace EBank.Application.Commands.Withdrawing
{
    /// <summary>
    /// 取消取款交易的应用层命令
    /// </summary>
    public class CancelWithdrawTransactionApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }
    }
}
