using System.Threading;
using System.Threading.Tasks;
using EBank.Domain.Models.Transferring;

namespace EBank.Application.Querying
{
    /// <summary>
    /// 银行账户查询服务
    /// </summary>
    public interface IBankAccountQueryService
    {
        Task<TransferTransactionAccount> GetAccountAsync(long accountId, CancellationToken token = default);
    }
}
