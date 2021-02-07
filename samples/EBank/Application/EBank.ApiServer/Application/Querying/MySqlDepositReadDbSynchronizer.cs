using EBank.Domain.Events.Depositing;
using EBank.Domain.Models.Depositing;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task OnDomainEventAsync(IDomainEventingContext context, DepositTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.AccountInTransactions}` (`Id`, `Type`, `AccountId`, `AccountName`, `Bank`, `Amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @Type, @AccountId, @AccountName, @Bank, @amount, @Status, @Creator, @CreatedTimestamp);";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Type = AccountTransactionType.Deposit.ToString(),
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

        public async Task OnDomainEventAsync(IDomainEventingContext context, DepositTransactionReadiedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.AccountInTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, DepositTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.AccountInTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString(),
                Message = @event.Message.SetEmptyStringWhenNull()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, DepositTransactionCancelledDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.AccountInTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";

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