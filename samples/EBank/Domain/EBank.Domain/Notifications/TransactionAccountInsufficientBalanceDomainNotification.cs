using EBank.Domain.Models.Accounts;
using MDA.Domain.Notifications;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 交易账户余额不足的领域通知
    /// </summary>
    public class TransactionAccountInsufficientBalanceDomainNotification : DomainNotification<long>
    {
        public TransactionAccountInsufficientBalanceDomainNotification(
            long transactionId,
            decimal transactionAmount, 
            decimal availableBalance, 
            AccountTransactionType transactionType, 
            AccountTransactionStage transactionStage)
        {
            TransactionId = transactionId;
            TransactionAmount = transactionAmount;
            AvailableBalance = availableBalance;
            TransactionType = transactionType;
            TransactionStage = transactionStage;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; private set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TransactionAmount { get; private set; }

        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal AvailableBalance { get; private set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public AccountTransactionType TransactionType { get; private set; }

        /// <summary>
        /// 交易阶段
        /// </summary>
        public AccountTransactionStage TransactionStage { get; private set; }
    }
}
