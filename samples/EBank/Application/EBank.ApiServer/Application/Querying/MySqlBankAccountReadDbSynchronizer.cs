using EBank.Domain.Events.Accounts;
using EBank.Domain.Models.Transferring;
using MDA.Domain.Events;
using MDA.StateBackend.RDBMS.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Querying
{
    public class MySqlBankAccountReadDbSynchronizer :
        IAsyncDomainEventHandler<AccountOpenedDomainEvent>,
        IAsyncDomainEventHandler<AccountNameChangedDomainEvent>,
        IAsyncDomainEventHandler<DepositTransactionSubmittedDomainEvent>,
        IAsyncDomainEventHandler<WithdrawTransactionSubmittedDomainEvent>,
        IAsyncDomainEventHandler<TransferTransactionSubmittedDomainEvent>
    {
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountReadDbSynchronizer(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, AccountOpenedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"INSERT INTO `{Tables.BankAccounts}` (`Id`, `Name`, `Bank`, `Balance`, `Creator`, `CreatedTimestamp`) VALUES (@Id, @Name, @Bank, @Balance, @Creator, @CreatedTimestamp);";

            var parameter = new
            {
                Id = @event.AggregateRootId.Id,
                Name = @event.AccountName.Name,
                Bank = @event.Bank.Name,
                Balance = @event.InitialBalance.Amount,
                Creator = "justmine",
                CreatedTimestamp = @event.Timestamp
            };

            await _db.ExecuteAsync(sql, parameter, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, AccountNameChangedDomainEvent @event, CancellationToken token)
        {
            var sql = $"UPDATE `{Tables.BankAccounts}` SET `Name`=@Name WHERE `Id`=@AccountId;";

            var po = new
            {
                AccountId = @event.AggregateRootId.Id,
                Name = @event.AccountName.Name
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, DepositTransactionSubmittedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.BankAccounts}` SET `Balance`=`Balance`+@Amount WHERE `Id`=@AccountId;";

            var po = new
            {
                AccountId = @event.AggregateRootId.Id,
                Amount = @event.Money.Amount
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, WithdrawTransactionSubmittedDomainEvent @event, CancellationToken token = default)
        {
            var sql = $"UPDATE `{Tables.BankAccounts}` SET `Balance`=`Balance`-@Amount WHERE `Id`=@AccountId;";

            var po = new
            {
                AccountId = @event.AggregateRootId.Id,
                Amount = @event.Money.Amount
            };

            await _db.ExecuteAsync(sql, po, token);
        }

        public async Task OnDomainEventAsync(IDomainEventingContext context, TransferTransactionSubmittedDomainEvent @event, CancellationToken token = default)
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
                AccountId = @event.AggregateRootId.Id,
                Amount = @event.Money.Amount
            };

            await _db.ExecuteAsync(sql, po, token);
        }
    }
}
