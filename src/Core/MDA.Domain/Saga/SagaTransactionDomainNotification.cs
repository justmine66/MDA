using MDA.Domain.Notifications;

namespace MDA.Domain.Saga
{
    public class SagaTransactionDomainNotification : DomainNotification
    {
        public SagaTransactionDomainNotification(string message, bool isSagaTransactionCompleted = false)
        {
            Message = message;
            IsCompleted = isSagaTransactionCompleted;
        }

        public string Message { get; set; }

        public bool IsCompleted { get; set; }
    }
}
