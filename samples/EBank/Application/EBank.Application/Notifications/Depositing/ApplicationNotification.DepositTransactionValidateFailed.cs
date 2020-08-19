using MDA.Application.Notifications;

namespace EBank.Application.Notifications.Depositing
{
    /// <summary>
    /// 存款交易信息验证失败的应用层通知
    /// </summary>
    public class DepositTransactionValidateFailedApplicationNotification : ApplicationNotification
    {
        public long TransactionId { get; set; }

        public string Reason { get; set; }
    }
}
