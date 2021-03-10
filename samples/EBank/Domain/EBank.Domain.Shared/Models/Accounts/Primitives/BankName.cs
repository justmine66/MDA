using MDA.Domain.Models;
using MDA.Infrastructure.Utils;
using System.Collections.Generic;

namespace EBank.Domain.Models.Accounts.Primitives
{
    /// <summary>
    /// 银行名称
    /// </summary>
    public class BankName : ValueObject
    {
        public BankName(string name)
        {
            PreConditions.Range(nameof(name), name.Length, BankAccount.Bank.Length.Minimum, BankAccount.Bank.Length.Maximum);

            Name = name;
        }

        public string Name { get; }

        public static implicit operator BankName(string name) => new BankName(name);
        public static implicit operator string(BankName name) => name.Name;

        public override string ToString() => Name;

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Name;
        }
    }
}
