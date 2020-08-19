using MDA.Domain.Exceptions;
using System;

namespace EBank.Domain.Models.Transferring
{
    public class TransferTransactionDomainException : DomainException
    {
        public TransferTransactionDomainException() { }
        public TransferTransactionDomainException(int code, string message) : base(code, message) { }
        public TransferTransactionDomainException(string message) : base(message) { }
        public TransferTransactionDomainException(string message, Exception e) : base(message, e) { }
    }
}
