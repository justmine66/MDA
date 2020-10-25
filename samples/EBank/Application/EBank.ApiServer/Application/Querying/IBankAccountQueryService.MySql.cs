using MDA.Shared.Utils;
using MDA.StateBackend.RDBMS.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EBank.ApiServer.Application.Querying.Models;

namespace EBank.ApiServer.Application.Querying
{
    public class MySqlBankAccountQueryService : IBankAccountQueryService
    {
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountQueryService(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.ReadDb);
        }

        public async Task<TransferAccountInfo> GetAccountAsync(long accountId, CancellationToken token = default)
        {
            var sql = $"SELECT `Id`,`Name`,`Bank` FROM {Tables.BankAccounts} WHERE `Id`=@Id";

            var records = await _db.ReadAsync<TransferAccountInfo>(sql, new
            {
                Id = accountId
            }, token);

            return records.IsEmpty() ? null : records.FirstOrDefault();
        }
    }
}
