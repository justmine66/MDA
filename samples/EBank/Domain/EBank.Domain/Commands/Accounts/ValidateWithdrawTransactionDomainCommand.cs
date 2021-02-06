using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 验证取款交易的领域命令
    /// </summary>
    public class ValidateWithdrawTransactionDomainCommand : SubTransactionDomainCommand<BankAccount, BankAccountId>
    {
        public ValidateWithdrawTransactionDomainCommand(
            WithdrawTransactionId transactionId,
            BankAccountId accountId,
            BankAccountName accountName,
            BankName bank,
            Money money)
        {
            TransactionId = transactionId;
            AggregateRootId = accountId;
            AccountName = accountName;
            Bank = bank;
            Money = money;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public WithdrawTransactionId TransactionId { get; }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; }

        /// <summary>
        /// 金额
        /// </summary>
        public Money Money { get; }
    }
}
