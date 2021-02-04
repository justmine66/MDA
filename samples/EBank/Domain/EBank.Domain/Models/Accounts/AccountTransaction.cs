using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using MDA.Domain.Models;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 表示一笔账户交易，比如：扣款、存款等。
    /// </summary>
    public class AccountTransaction : Entity<AccountTransactionId>
    {
        public AccountTransaction(
            AccountTransactionId transactionId,
            Money money,
            AccountFundDirection fundDirection) 
            : base(transactionId)
        {
            Money = money;
            FundDirection = fundDirection;
        }

        /// <summary>
        /// 交易金额
        /// </summary>
        public Money Money { get;  }

        /// <summary>
        /// 资金流向
        /// </summary>
        public AccountFundDirection FundDirection { get;  }
    }
}
