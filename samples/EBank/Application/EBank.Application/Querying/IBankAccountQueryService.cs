﻿using System.Threading;
using System.Threading.Tasks;
using EBank.Application.Querying.Models;

namespace EBank.Application.Querying
{
    /// <summary>
    /// 银行账户查询服务
    /// </summary>
    public interface IBankAccountQueryService
    {
        Task<TransferAccountInfo> GetAccountAsync(long accountId, CancellationToken token = default);
    }
}
