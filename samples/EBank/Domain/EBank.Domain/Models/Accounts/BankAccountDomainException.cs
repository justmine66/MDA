using MDA.Domain.Exceptions;
using System;

namespace EBank.Domain.Models.Accounts
{
    public class BankAccountDomainException : DomainException
    {
        public BankAccountDomainException() { }
        public BankAccountDomainException(int code, string message) : base(code, message) { }
        public BankAccountDomainException(string message) : base(message) { }
        public BankAccountDomainException(string message, Exception e) : base(message, e) { }
    }
}
