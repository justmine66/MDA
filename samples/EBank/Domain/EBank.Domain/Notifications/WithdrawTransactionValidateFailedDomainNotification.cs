using MDA.Domain.Notifications;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 取款交易验证失败的领域通知
    /// </summary>
    public class WithdrawTransactionValidateFailedDomainNotification : DomainNotification<long>
    {
        public WithdrawTransactionValidateFailedDomainNotification(long transactionId, string reason)
        {
            TransactionId = transactionId;
            Reason = reason;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; }
    }
}
