using MDA.Domain.Models;
using MDA.Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace EBank.Domain.Models.Primitives
{
    public class Money : ValueObject
    {
        public Money(decimal amount, Currency currency)
        {
            PreConditions.NotNull(currency, nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }

        public Currency Currency { get; }

        public static implicit operator Money(decimal amount) => new Money(amount, Currency.Default);
        public static implicit operator decimal(Money money) => money.Amount;

        public static Money operator +(Money left, Money right)
        {
            if (!left.Currency.Equals(right.Currency))
            {
                throw new NotSupportedException();
            }

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (!left.Currency.Equals(right.Currency))
            {
                throw new NotSupportedException();
            }

            return new Money(left.Amount - right.Amount, left.Currency);
        }

        public static Money operator *(Money left, Money right)
        {
            if (!left.Currency.Equals(right.Currency))
            {
                throw new NotSupportedException();
            }

            return new Money(left.Amount * right.Amount, left.Currency);
        }

        public static Money operator /(Money left, Money right)
        {
            if (!left.Currency.Equals(right.Currency))
            {
                throw new NotSupportedException($"The left currency: {left.Currency}, is not equal to the right currency: {right.Currency}.");
            }

            if (right == decimal.Zero)
            {
                throw new NotSupportedException("The divisor cannot be zero.");
            }

            return new Money(left.Amount / right.Amount, left.Currency);
        }

        public override string ToString() => $"[Amount: {Amount}, Currency: {Currency}]";

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
