using EBank.Domain.Models.Accounts;
using MDA.Application.Commands;
using MDA.Domain.Shared;

namespace EBank.Application.Commands.Accounts
{
    /// <summary>
    /// 发起存款账户交易
    /// </summary>
    public class StartDepositAccountTransactionApplicationCommand : ApplicationCommand
    {
        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; set; }

        /// <summary>
        /// 账户号
        /// </summary>
        public long AccountId { get; set; } = SnowflakeId.Default().NextId();

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

        /// <summary>
        /// 账户交易阶段
        /// </summary>
        public AccountTransactionStage TransactionStage { get; set; }
    }
}
