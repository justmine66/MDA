using EBank.ApiServer.Models.Output.BankAccounts;
using MDA.Infrastructure.Utils;
using MDA.StateBackend.RDBMS.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Querying
{
    public class MySqlBankAccountQueryService : IBankAccountQueryService
    {
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountQueryService(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task<GetBankAccountOutput> GetAccountAsync(long accountId, CancellationToken token = default)
        {
            var sql = $"SELECT `Id`,`Name`,`Bank`,`Balance` FROM {Tables.BankAccounts} WHERE `Id`=@Id";

            var records = await _db.ReadAsync<GetBankAccountOutput>(sql, new
            {
                Id = accountId
            }, token);

            return records.IsEmpty() ? null : records.FirstOrDefault();
        }

        public async Task<bool> HasAccountAsync(long accountId, CancellationToken token = default)
        {
            var sql = $"SELECT 1 FROM {Tables.BankAccounts} WHERE `Id`=@Id";

            var hasAccount = await _db.ReadAsync<bool>(sql, new
            {
                Id = accountId
            }, token);

            return hasAccount.IsNotEmpty();
        }
    }
}
