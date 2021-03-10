using MDA.Application.Commands;

namespace EBank.Application.Commanding.Depositing
{
    /// <summary>
    /// 发起存款的应用命令
    /// </summary>
    public class StartDepositApplicationCommand : ApplicationCommand
    {
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

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
