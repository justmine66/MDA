using EBank.Domain.Events.Accounts;
using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Querying
{
    public class MySqlBankAccountReadDbSynchronizer :
        IAsyncDomainEventHandler<AccountOpenedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionSubmittedDomainEvent>,
        IAsyncDomainEventHandler<WithdrawTransactionSubmittedDomainEvent>,
        IAsyncDomainEventHandler<TransferTransactionSubmittedDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountReadDbSynchronizer(
            ILogger<MySqlBankAccountReadDbSynchronizer> logger,
            IRelationalDbStorageFactory db)
        {
            _logger = logger;
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task HandleAsync(AccountOpenedDomainEvent @event, CancellationToken token = default)
        {
            var sql =
                $"INSERT INTO `{Tables.BankAccounts}` (`Id`, `Name`, `Bank`, `Balance`, `Creator`, `CreatedTimestamp`) VALUES (@Id, @Name, @Bank, @Balance, @Creator, @CreatedTimestamp);";

            var parameter = new
            {
                Id = @event.AggregateRootId,
                Name = @event.AccountName,
                Bank = @event.Bank,
                Balance = @event.InitialBalance,
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, parameter, token);
        }

        public async Task HandleAsync(DepositTransactionSubmittedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.BankAccounts}` SET `Balance`=`Balance`+@Amount WHERE `Id`=@AccountId;";

            var po = new
            {
                AccountId = @event.AggregateRootId,
                Amount = @event.Amount
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(WithdrawTransactionSubmittedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.BankAccounts}` SET `Balance`=`Balance`-@Amount WHERE `Id`=@AccountId;";

            var po = new
            {
                AccountId = @event.AggregateRootId,
                Amount = @event.Amount
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task HandleAsync(TransferTransactionSubmittedDomainEvent @event, CancellationToken token = default)
        {
            var sql = string.Empty;
            switch (@event.AccountType)
            {
                case TransferAccountType.Source:
                    sql = $"UPDATE `{Tables.BankAccounts}` SET `Balance`=`Balance`-@Amount WHERE `Id`=@AccountId;";
                    break;
                case TransferAccountType.Sink:
                    sql = $"UPDATE `{Tables.BankAccounts}` SET `Balance`=`Balance`+@Amount WHERE `Id`=@AccountId;";
                    break;
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                return;
            }

            var po = new
            {
                AccountId = @event.AggregateRootId,
                Amount = @event.Amount
            };

            await _db.ExecuteAsync(sql, po, token);
        }
    }
}
