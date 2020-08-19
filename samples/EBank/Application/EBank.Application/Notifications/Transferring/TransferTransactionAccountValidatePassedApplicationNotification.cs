using EBank.Domain.Models.Transferring;
using MDA.Application.Notifications;

namespace EBank.Application.Notifications.Transferring
{
    public class TransferTransactionAccountValidatePassedApplicationNotification : ApplicationNotification
    {
        public long TransactionId { get; set; }

        public TransferTransactionAccountType AccountType { get; set; }
    }
}
