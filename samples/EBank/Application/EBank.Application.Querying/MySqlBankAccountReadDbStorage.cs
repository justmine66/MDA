using EBank.Domain.Events.Accounts;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application.Querying
{
    public class MySqlBankAccountReadDbStorage :
        IAsyncDomainEventHandler<AccountOpenedDomainEvent>,
        IAsyncDomainEventHandler<DepositAccountTransactionCompletedDomainEvent>,
        IAsyncDomainEventHandler<WithdrawAccountTransactionCompletedDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountReadDbStorage(
            ILogger<MySqlBankAccountReadDbStorage> logger,
            IRelationalDbStorage db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task HandleAsync(AccountOpenedDomainEvent @event, CancellationToken token = default)
        {
            var sql =
                $"INSERT INTO `{Tables.BankAccounts}` (`Id`, `Name`, `Bank`, `Balance`, `Creator`, `CreatedTime`) VALUES (@Id, @Name, @Bank, @Balance, @Creator, @CreatedTime);";

            var parameter = new
            {
                Id = @event.AggregateRootId,
                Name = @event.AccountName,
                Bank = @event.Bank,
                Balance = @event.InitialBalance,
                Creator = "justmine",
                CreatedTime = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, parameter, token);
        }

        public async Task HandleAsync(DepositAccountTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task HandleAsync(WithdrawAccountTransactionCompletedDomainEvent @event, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
