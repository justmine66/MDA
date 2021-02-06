using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Withdrawing;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Saga;

namespace EBank.Domain.Commands.Withdrawing
{
    /// <summary>
    /// 发起取款交易的领域命令
    /// </summary>
    public class StartWithdrawTransactionDomainCommand : BeginSubTransactionDomainCommand<WithdrawTransaction, WithdrawTransactionId>
    {
        public StartWithdrawTransactionDomainCommand(
            WithdrawTransactionId transactionId,
            BankAccountId accountId,
            BankAccountName accountName,
            Money money,
            BankName bank)
        {
            AggregateRootId = transactionId;
            AccountId = accountId;
            AccountName = accountName;
            Money = money;
            Bank = bank;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public BankAccountId AccountId { get; }

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
