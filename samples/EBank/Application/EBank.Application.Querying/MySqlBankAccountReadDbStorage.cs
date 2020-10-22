using EBank.Domain.Events.Accounts;
using EBank.Domain.Events.Depositing;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application.Querying
{
    public class MySqlBankAccountReadDbStorage :
        IAsyncDomainEventHandler<AccountOpenedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionSubmittedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionCompletedDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountReadDbStorage(
            ILogger<MySqlBankAccountReadDbStorage> logger,
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
            throw new System.NotImplementedException();
        }

        public async Task HandleAsync(DepositTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
