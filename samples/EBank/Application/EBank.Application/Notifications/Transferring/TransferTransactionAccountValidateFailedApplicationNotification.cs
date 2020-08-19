using EBank.Domain.Models.Transferring;
using MDA.Application.Notifications;

namespace EBank.Application.Notifications.Transferring
{
    public class TransferTransactionAccountValidateFailedApplicationNotification : ApplicationNotification
    {
        public long TransactionId { get; set; }

        public TransferTransactionAccount Account { get; set; }

        public string Reason { get; set; }

        public TransferTransactionAccountType AccountType { get; set; }
    }
}
