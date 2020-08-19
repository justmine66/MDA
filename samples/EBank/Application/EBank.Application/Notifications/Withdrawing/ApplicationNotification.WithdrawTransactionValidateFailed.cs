using MDA.Application.Notifications;

namespace EBank.Application.Notifications.Withdrawing
{
    /// <summary>
    /// 取款交易信息验证失败的应用层通知
    /// </summary>
    public class WithdrawTransactionValidateFailedApplicationNotification : ApplicationNotification
    {
        public long TransactionId { get; set; }

        public string Reason { get; set; }
    }
}
