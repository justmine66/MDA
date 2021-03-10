using EBank.Domain.Models.Depositing.Primitives;
using EBank.Domain.Models.Transferring.Primitives;
using EBank.Domain.Models.Withdrawing.Primitives;
using MDA.Domain.Models;
using MDA.Infrastructure.Utils;

namespace EBank.Domain.Models.Accounts.Primitives
{
    /// <summary>
    /// 账户交易标识
    /// </summary>
    public class AccountTransactionId : Identity<long>
    {
        public AccountTransactionId(long id) : base(id)
        {
            PreConditions.Positive(id, nameof(id));
        }

        public static implicit operator AccountTransactionId(long id) => new AccountTransactionId(id);
        public static implicit operator AccountTransactionId(DepositTransactionId id) => new AccountTransactionId(id);
        public static implicit operator AccountTransactionId(WithdrawTransactionId id) => new AccountTransactionId(id);
        public static implicit operator AccountTransactionId(TransferTransactionId id) => new AccountTransactionId(id);
        public static implicit operator long(AccountTransactionId id) => id.Id;
    }
}
