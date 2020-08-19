using MDA.Application.Commands;

namespace EBank.Application.Commands.Depositing
{
    /// <summary>
    /// 取消存款交易的应用层命令
    /// </summary>
    public class CancelDepositTransactionApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }
    }
}
