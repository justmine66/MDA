using EBank.Domain.Models.Transferring;
using MDA.Domain.Notifications;

namespace EBank.Domain.Notifications
{
    /// <summary>
    /// 转账交易验证失败的领域通知
    /// </summary>
    public class TransferTransactionValidateFailedDomainNotification : DomainNotification<long>
    {
        public TransferTransactionValidateFailedDomainNotification(long transactionId, TransferAccountType accountType, string reason)
        {
            TransactionId = transactionId;
            AccountType = accountType;
            Reason = reason;
        }

        /// <summary>
        /// 交易标识
        /// </summary>
        public long TransactionId { get; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public TransferAccountType AccountType { get; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; }
    }
}
