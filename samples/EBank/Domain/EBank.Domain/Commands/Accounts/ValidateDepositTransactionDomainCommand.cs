using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 验证存款交易信息的领域命令
    /// </summary>
    public class ValidateDepositTransactionDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public DepositTransactionId TransactionId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public Money Amount { get; set; }
    }
}
