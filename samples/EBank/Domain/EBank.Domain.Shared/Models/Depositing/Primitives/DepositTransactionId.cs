using EBank.Domain.Models.Accounts.Primitives;
using MDA.Domain.Models;
using MDA.Infrastructure.Utils;

namespace EBank.Domain.Models.Depositing.Primitives
{
    /// <summary>
    /// 存款交易标识
    /// </summary>
    public class DepositTransactionId : Identity<long>
    {
        public DepositTransactionId(long id) : base(id)
        {
            PreConditions.Positive(id, nameof(id));
        }

        public static implicit operator DepositTransactionId(long id) => new DepositTransactionId(id);
        public static implicit operator DepositTransactionId(AccountTransactionId id) => new DepositTransactionId(id);
        public static implicit operator long(DepositTransactionId id) => id.Id;
    }
}
