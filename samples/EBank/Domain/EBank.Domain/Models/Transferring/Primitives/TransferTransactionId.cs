using EBank.Domain.Models.Accounts.Primitives;
using MDA.Domain.Models;
using MDA.Infrastructure.Utils;

namespace EBank.Domain.Models.Transferring.Primitives
{
    /// <summary>
    /// 转账交易标识
    /// </summary>
    public class TransferTransactionId : Identity<long>
    {
        public TransferTransactionId(long id) : base(id)
        {
            PreConditions.Positive(id, nameof(id));
        }

        public static implicit operator TransferTransactionId(long id) => new TransferTransactionId(id);
        public static implicit operator TransferTransactionId(AccountTransactionId id) => new TransferTransactionId(id);
        public static implicit operator long(TransferTransactionId id) => id.Id;
    }
}