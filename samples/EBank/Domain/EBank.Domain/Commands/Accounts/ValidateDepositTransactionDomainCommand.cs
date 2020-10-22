using EBank.Domain.Models.Accounts;
using MDA.Domain.Commands;

namespace EBank.Domain.Commands.Accounts
{
    /// <summary>
    /// 验证存款交易信息的领域命令
    /// </summary>
    public class ValidateDepositTransactionDomainCommand : DomainCommand<BankAccount, long>
    {
        public ValidateDepositTransactionDomainCommand(
            long transactionId, 
            long accountId,
            string accountName, 
            string bank, 
            decimal amount)
        {
            TransactionId = transactionId;
            AggregateRootId = accountId;
            AccountName = accountName;
            Bank = bank;
            Amount = amount;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get;  }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get;  }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get;  }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get;  }
    }
}
