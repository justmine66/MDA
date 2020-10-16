using EBank.Domain.Models.Accounts;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 开户命令
    /// </summary>
    public class OpenAccountDomainCommand : DomainCommand<BankAccount, long>
    {
        public OpenAccountDomainCommand(
            long accountId,
            string accountName,
            string bank,
            decimal initialBalance)
        {
            AggregateRootId = accountId;
            AccountName = accountName;
            Bank = bank;
            InitialBalance = initialBalance;
        }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public decimal InitialBalance { get; set; }
    }
}
