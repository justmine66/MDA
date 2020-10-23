using EBank.Domain.Events.Depositing;
using EBank.Domain.Models.Depositing;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application.Querying
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

        public async Task HandleAsync(DepositTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.DepositTransactions}` (`Id`, `AccountId`, `AccountName`, `Bank`, `Amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @AccountId, @AccountName, @Bank, @amount, @Status, @Creator, @CreatedTimestamp);";

            var po = new 
            {
                TransactionId = @event.AggregateRootId,
                AccountId = @event.AccountId,
                AccountName = @event.AccountName,
                Amount = @event.Amount,
                Bank = @event.Bank,
                Status = DepositTransactionStatus.Started.ToString(),
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(DepositTransactionReadiedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.DepositTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(DepositTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.DepositTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(DepositTransactionCancelledDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.DepositTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }
    }
}