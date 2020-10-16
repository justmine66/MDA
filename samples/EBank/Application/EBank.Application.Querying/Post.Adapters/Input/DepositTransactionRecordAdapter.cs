using EBank.Domain.Events.Depositing;
using EBank.Domain.Models.Depositing;

namespace EBank.Application.Querying.Post.Adapters.Input
{
    public static class DepositTransactionRecordAdapter
    {
        public static DepositTransactionRecord ToDepositTransactionRecord(DepositTransactionStartedDomainEvent @event)
        {
            return new DepositTransactionRecord()
            {
                TransactionId = @event.Id,
                AccountId = @event.AccountId,
                AccountName = @event.AccountName,
                Amount = @event.Amount,
                Bank = @event.Bank,
                Status = DepositTransactionStatus.Started.ToString()
            };
        }
    }
}
