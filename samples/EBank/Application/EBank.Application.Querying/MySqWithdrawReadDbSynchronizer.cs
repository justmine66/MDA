﻿using EBank.Domain.Events.Withdrawing;
using EBank.Domain.Models.Withdrawing;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application.Querying
{
    public class MySqWithdrawReadDbSynchronizer :
        IAsyncDomainEventHandler<WithdrawTransactionStartedDomainEvent>,
        IAsyncDomainEventHandler<WithdrawTransactionReadiedDomainEvent>,
        IAsyncDomainEventHandler<WithdrawTransactionCompletedDomainEvent>,
        IAsyncDomainEventHandler<WithdrawTransactionCancelledDomainEvent>
    {
        private readonly IRelationalDbStorage _db;

        public MySqWithdrawReadDbSynchronizer(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task HandleAsync(WithdrawTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.WithdrawTransactions}` (`Id`, `AccountId`, `AccountName`, `Bank`, `Amount`, `Status`, `Creator`, `CreatedTimestamp`) VALUES (@TransactionId, @AccountId, @AccountName, @Bank, @amount, @Status, @Creator, @CreatedTimestamp);";

            var po = new 
            {
                TransactionId = @event.AggregateRootId,
                AccountId = @event.AccountId,
                AccountName = @event.AccountName,
                Amount = @event.Amount,
                Bank = @event.Bank,
                Status = WithdrawTransactionStatus.Started.ToString(),
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(WithdrawTransactionReadiedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.WithdrawTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(WithdrawTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.WithdrawTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(WithdrawTransactionCancelledDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.WithdrawTransactions}` SET `Status`=@Status WHERE `Id`=@TransactionId;";

            var po = new
            {
                TransactionId = @event.AggregateRootId,
                Status = @event.Status.ToString()
            };

            await _db.ExecuteAsync(sql, po, token);
        }
    }
}