using MDA.Application.Commands;

namespace EBank.Application.Commands.Withdrawing
{
    /// <summary>
    /// 验证取款交易信息的应用层命令
    /// </summary>
    public class ValidateWithdrawTransactionApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }
    }
}
