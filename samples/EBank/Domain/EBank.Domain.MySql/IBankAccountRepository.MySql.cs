using System.Threading.Tasks;
using EBank.Domain.Models.Accounts;
using MDA.StateBackend.RDBMS.Shared;

namespace EBank.Domain.MySql
{
    public class MySqlBankAccountRepository : IBankAccountRepository
    {
        private readonly IRelationalDbStorage _db;

        public MySqlBankAccountRepository(IRelationalDbStorageFactory db)
        {
            _db = db.CreateRelationalDbStorage(DatabaseScheme.StateDb);
        }

        public async Task<bool> HadAccountNameAsync(string name)
        {
            // todo: 从状态库查询。

            return await Task.FromResult(false);
        }
    }
}
