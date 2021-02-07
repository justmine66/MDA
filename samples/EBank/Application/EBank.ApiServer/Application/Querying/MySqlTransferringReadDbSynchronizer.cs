using EBank.Domain.Events.Transferring;
using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Querying
{
    public class MySqlTransferringReadDbSynchronizer :
        IAsyncDomainEventHandler<TransferTransactionStartedDomainEvent>,
        IAsyncDomainEventHandler<TransferTransactionReadiedDomainEvent>,
        IAsyncDomainEventHandler<TransferTransactionCompletedDomainEvent>,
        IAsyncDomainEventHandler<TransferTransactionCancelledDomainEvent>
    {
        private readonly IRelationalDbStorage _db;

        public MySqlTransferringReadDbSynchronizer(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.TransferTransactions}` (`Id`, `SourceAccountId`, `SourceAccountName`, `SourceBank`, `SinkAccountId`, `SinkAccountName`, `SinkBank`, `amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @SourceAccountId, @SourceAccountName, @SourceBank, @SinkAccountId, @SinkAccountName, @SinkBank, @amount, @Status, @Creator, @CreatedTimestamp);";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                SourceAccountId = @event.SourceAccount.Id.Id,
                SourceAccountName = @event.SourceAccount.Name.Name,
                SourceBank = @event.SourceAccount.Bank.Name,
                SinkAccountId = @event.SinkAccount.Id.Id,
                SinkAccountName = @event.SinkAccount.Name.Name,
                SinkBank = @event.SinkAccount.Bank.Name,
                Amount = @event.Money.Amount,
                Status = TransferTransactionStatus.Started.ToString(),
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionReadiedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.TransferTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.TransferTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionCancelledDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.TransferTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString(),
                Message = @event.Message.SetEmptyStringWhenNull()
            };

            await _db.ExecuteAsync(sql, po, token);
        }
    }
}