using MDA.Application.Commands;

namespace EBank.Application.Commands.Depositing
{
    /// <summary>
    /// 确认存款交易信息验证通过的应用层命令
    /// </summary>
    public class ConfirmDepositTransactionValidatePassedApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }
    }
}
