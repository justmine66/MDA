using MDA.Domain.Models;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 表示一笔账户交易，比如：扣款、存款等。
    /// </summary>
    public class AccountTransaction : Entity<long>
    {
        public AccountTransaction(
            long transactionId,
            decimal amount,
            AccountFundDirection fundDirection) 
            : base(transactionId)
        {
            Amount = amount;
            FundDirection = fundDirection;
        }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get;  }

        /// <summary>
        /// 资金流向
        /// </summary>
        public AccountFundDirection FundDirection { get;  }
    }
}
