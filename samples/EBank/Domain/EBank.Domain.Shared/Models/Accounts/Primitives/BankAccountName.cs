using MDA.Domain.Models;
using MDA.Infrastructure.Utils;
using System.Collections.Generic;

namespace EBank.Domain.Models.Accounts.Primitives
{
    /// <summary>
    /// 银行账户名称
    /// </summary>
    public class BankAccountName : ValueObject
    {
        public BankAccountName(string name)
        {
            PreConditions.Range(nameof(name), name.Length, BankAccount.Name.Length.Minimum, BankAccount.Name.Length.Maximum);

            Name = name;
        }

        public string Name { get; }

        public static implicit operator BankAccountName(string name) => new BankAccountName(name);
        public static implicit operator string(BankAccountName name) => name.Name;

        public override string ToString() => Name;

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Name;
        }
    }
}
