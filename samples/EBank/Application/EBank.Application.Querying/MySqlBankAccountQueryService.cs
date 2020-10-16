using EBank.Domain.Models.Transferring;
using System.Threading.Tasks;

namespace EBank.Application.Querying
{
    public class MySqlBankAccountQueryService : IBankAccountQueryService
    {
        public async Task<TransferTransactionAccount> GetAccountAsync(long accountId)
        {
            return await Task.FromResult(new TransferTransactionAccount(1, "justmine", "招商"));
        }
    }
}
