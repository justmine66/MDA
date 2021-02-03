using EBank.Domain.Models.Accounts;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 验证存款交易信息的领域命令
    /// </summary>
    public class ValidateDepositTransactionDomainCommand : DomainCommand<BankAccount, long>
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
