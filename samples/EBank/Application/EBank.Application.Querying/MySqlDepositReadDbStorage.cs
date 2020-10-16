using EBank.Application.Querying.Post.Adapters.Input;
using EBank.Domain.Events.Depositing;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application.Querying
{
    public class MySqlDepositReadDbStorage :
        IAsyncDomainEventHandler<DepositTransactionStartedDomainEvent>
    {
        private readonly ILogger _logger;
        private readonly IRelationalDbStorage _db;

        public MySqlDepositReadDbStorage(
            ILogger<MySqlDepositReadDbStorage> logger,
            IRelationalDbStorage db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task HandleAsync(DepositTransactionStartedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.BankAccounts}` (`Id`, `Name`, `Bank`, `Balance`, `Creator`, `CreatedTime`) VALUES (@Id, @Name, @Bank, @Balance, @Creator, @CreatedTime);";

            var record = DepositTransactionRecordAdapter.ToDepositTransactionRecord(@event);

            await _db.ExecuteAsync(sql, record, token);
        }
    }
}