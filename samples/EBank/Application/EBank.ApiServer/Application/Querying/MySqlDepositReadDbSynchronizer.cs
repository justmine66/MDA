using EBank.Domain.Events.Depositing;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using System.Threading;
using System.Threading.Tasks;
using EBank.Domain.Models.Depositing;

namespace EBank.ApiServer.Application.Querying
{
    public class MySqlDepositReadDbSynchronizer :
        IAsyncDomainEventHandler<DepositTransactionStartedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionReadiedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionCompletedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionCancelledDomainEvent>
    {
        private readonly IRelationalDbStorage _db;

        public MySqlDepositReadDbSynchronizer(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task OnDomainEventAsync(IDomainEventHandlingContext context, DepositTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.DepositTransactions}` (`Id`, `AccountId`, `AccountName`, `Bank`, `Amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @AccountId, @AccountName, @Bank, @amount, @Status, @Creator, @CreatedTimestamp);";

            var po = new 
            {
                TransactionId = @event.AggregateRootId.Id,
                AccountId = @event.AccountId.Id,
                AccountName = @event.AccountName.Name,
                Amount = @event.Money.Amount,
                Bank = @event.Bank.Name,
                Status = DepositTransactionStatus.Started.ToString(),
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventHandlingContext context, DepositTransactionReadiedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.DepositTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventHandlingContext context, DepositTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.DepositTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventHandlingContext context, DepositTransactionCancelledDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.DepositTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }
    }
}