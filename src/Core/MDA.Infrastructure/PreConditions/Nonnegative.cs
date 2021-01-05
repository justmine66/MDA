using System;

namespace MDA.Infrastructure.PreConditions
{
    public class Nonnegative<T>
    {
        public Nonnegative(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public static implicit operator Nonnegative<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException();

            switch (value)
            {
                case short shotValue:
                    if (shotValue <= 0)
                        throw new ArgumentOutOfRangeException($"{nameof(value)} should be positive.");
                    break;
                case int intValue:
                    if (intValue <= 0)
                        throw new ArgumentOutOfRangeException($"{nameof(value)} should be positive.");
                    break;
                case long longValue:
                    if (longValue <= 0)
                        throw new ArgumentOutOfRangeException($"{nameof(value)} should be positive.");
                    break;
                case float floatValue:
                    if (floatValue <= 0)
                        throw new ArgumentOutOfRangeException($"{nameof(value)} should be positive.");
                    break;
                case double doubleValue:
                    if (doubleValue <= 0)
                        throw new ArgumentOutOfRangeException($"{nameof(value)} should be positive.");
                    break;
                case decimal decimalValue:
                    if (decimalValue <= 0)
                        throw new ArgumentOutOfRangeException($"{nameof(value)} should be positive.");
                    break;
                default:
                    throw new ArgumentNullException();
            }

            return new Nonnegative<T>(value);
        }

        public static implicit operator T(Nonnegative<T> value) => value.Value;
    }
}
