using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 开户命令
    /// </summary>
    public class OpenAccountDomainCommand : DomainCommand<long>
    {
        public OpenAccountDomainCommand(
            string accountName,
            string bank,
            decimal initialBalance) 
        {
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
