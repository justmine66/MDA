using EBank.Domain.Models.Accounts.Primitives;
using MDA.Domain.Models;
using MDA.Infrastructure.Utils;

namespace EBank.Domain.Models.Withdrawing.Primitives
{
    /// <summary>
    /// 取款交易标识
    /// </summary>
    public class WithdrawTransactionId : Identity<long>
    {
        public WithdrawTransactionId(long id) : base(id)
        {
            PreConditions.Positive(id, nameof(id));
        }

        public static implicit operator WithdrawTransactionId(long id) => new WithdrawTransactionId(id);
        public static implicit operator WithdrawTransactionId(AccountTransactionId id) => new WithdrawTransactionId(id);
        public static implicit operator long(WithdrawTransactionId id) => id.Id;
    }
}