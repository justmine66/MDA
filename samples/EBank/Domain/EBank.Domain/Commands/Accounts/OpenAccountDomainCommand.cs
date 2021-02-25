using EBank.Domain.Models.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Shared.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 开户命令
    /// </summary>
    public class OpenAccountDomainCommand : DomainCommand<BankAccount, BankAccountId>
    {
        public OpenAccountDomainCommand(
            BankAccountId accountId,
            BankAccountName accountName,
            BankName bank,
            Money initialBalance)
        {
            AggregateRootId = accountId;
            AccountName = accountName;
            Bank = bank;
            InitialBalance = initialBalance;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public Money InitialBalance { get; set; }
    }
}
