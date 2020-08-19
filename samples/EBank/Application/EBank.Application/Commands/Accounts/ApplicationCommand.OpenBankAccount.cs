using MDA.Application.Commands;

namespace EBank.Application.Commands.Accounts
{
    public class OpenBankAccountApplicationCommand : ApplicationCommand
    {
        public OpenBankAccountApplicationCommand(
            long accountId,
            string accountName,
            string bank,
            decimal initialBalance)
        {
            AccountId = accountId;
            AccountName = accountName;
            Bank = bank;
            InitialBalance = initialBalance;
        }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; private set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; private set; }

        /// <summary>
        /// 初始余额
        /// </summary>
        public decimal InitialBalance { get; private set; }
    }
}
