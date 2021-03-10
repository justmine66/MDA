using MDA.Domain.Models;
using MDA.Infrastructure.Utils;

namespace EBank.Domain.Models.Accounts.Primitives
{
    /// <summary>
    /// 银行账户标识
    /// </summary>
    public class BankAccountId : Identity<long>
    {
        public BankAccountId(long id) : base(id)
        {
            PreConditions.Positive(id, nameof(id));
        }

        public static implicit operator BankAccountId(long id) => new BankAccountId(id);
        public static implicit operator long(BankAccountId id) => id.Id;
    }
}
