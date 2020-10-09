using EBank.Domain.Models.Accounts;
using System.Threading.Tasks;

namespace EBank.Domain.MySql
{
    public class MySqlBankAccountRepository : IBankAccountRepository
    {
        public async Task<bool> HadAccountNameAsync(string name)
        {
            return await Task.FromResult(false);
        }
    }
}
