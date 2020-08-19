using MDA.Application.Commands;

namespace EBank.Application.Commands.Transferring
{
    /// <summary>
    /// 取消转账交易的应用层命令
    /// </summary>
    public class CancelTransferTransactionApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }
    }
}
