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
            var insertWithdrawSql = $"INSERT INTO `{Tables.AccountOutTransactions}` (`Id`, `Type`, `AccountId`, `AccountName`, `Bank`, `Amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @Type, @AccountId, @AccountName, @Bank, @amount, @Status, @Creator, @CreatedTimestamp);";
            var insertDepositSql = $"INSERT INTO `{Tables.AccountInTransactions}` (`Id`, `Type`, `AccountId`, `AccountName`, `Bank`, `Amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @Type, @AccountId, @AccountName, @Bank, @amount, @Status, @Creator, @CreatedTimestamp);";
           
            await _db.ExecuteAsync(insertWithdrawSql, new
            {
                TransactionId = @event.AggregateRootId.Id,
                Type = AccountTransactionType.Transfer.ToString(),
                AccountId = @event.SourceAccount.Id.Id,
                AccountName = @event.SourceAccount.Name.Name,
                Bank = @event.SourceAccount.Bank.Name,
                Amount = @event.Money.Amount,
                Status = TransferTransactionStatus.Started.ToString(),
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            }, token);

            await _db.ExecuteAsync(insertDepositSql, new
            {
                TransactionId = @event.AggregateRootId.Id,
                Type = AccountTransactionType.Transfer.ToString(),
                AccountId = @event.SinkAccount.Id.Id,
                AccountName = @event.SinkAccount.Name.Name,
                Bank = @event.SinkAccount.Bank.Name,
                Amount = @event.Money.Amount,
                Status = TransferTransactionStatus.Started.ToString(),
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            }, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionReadiedDomainEvent @event, CancellationToken token = default)
        {
            var updateWithdrawSql = $"UPDATE `{Tables.AccountOutTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";
            var updateDepositSql = $"UPDATE `{Tables.AccountInTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(updateWithdrawSql, po, token);
            await _db.ExecuteAsync(updateDepositSql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            switch (@event.AccountType)
            {
                case TransferAccountType.Source:
                    var updateWithdrawSql = $"UPDATE `{Tables.AccountOutTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";

                    await _db.ExecuteAsync(updateWithdrawSql, new
                    {
                        TransactionId = @event.AggregateRootId.Id,
                        Status = @event.Status.ToString(),
                        Message = @event.Message.SetEmptyStringWhenNull()
                    }, token);
                    break;

                case TransferAccountType.Sink:
                    var updateDepositSql = $"UPDATE `{Tables.AccountInTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";

                    await _db.ExecuteAsync(updateDepositSql, new
                    {
                        TransactionId = @event.AggregateRootId.Id,
                        Status = @event.Status.ToString(),
                        Message = @event.Message.SetEmptyStringWhenNull()
                    }, token);
                    break;
            }
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionCancelledDomainEvent @event, CancellationToken token = default)
        {
            var updateWithdrawSql = $"UPDATE `{Tables.AccountOutTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";
            var updateDepositSql = $"UPDATE `{Tables.AccountInTransactions}` SET `Status`=@Status,`Message`=@Message WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId.Id,
                Status = @event.Status.ToString(),
                Message = @event.Message.SetEmptyStringWhenNull()
            };

            await _db.ExecuteAsync(updateWithdrawSql, po, token);
            await _db.ExecuteAsync(updateDepositSql, po, token);
        }
    }
}