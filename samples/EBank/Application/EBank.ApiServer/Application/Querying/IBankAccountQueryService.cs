using System.Threading;
using System.Threading.Tasks;
using EBank.ApiServer.Models.Output.BankAccounts;

namespace EBank.ApiServer.Application.Querying
{
    /// <summary>
    /// 银行账户查询服务
    /// </summary>
    public interface IBankAccountQueryService
    {
        Task<GetBankAccountOutput> GetAccountAsync(long accountId, CancellationToken token = default);

        Task<bool> HasAccountAsync(long accountId, CancellationToken token = default);
    }
}
