using MDA.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace MDA.Domain.Shared
{
    /// <summary>
    /// 预置条件检查帮组类
    /// </summary>
    public static class PreConditions
    {
        public static void GreaterThanZero(string name, float value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{name} must be positive.");
            }
        }
        public static void GreaterThanZero<TSubject>(string name, float value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be positive.");
            }
        }

        public static void GreaterThanZero(string name, double value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{name} must be positive.");
            }
        }
        public static void GreaterThanZero<TSubject>(string name, double value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be positive.");
            }
        }

        public static void GreaterThanZero(string name, decimal value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{name} must be positive.");
            }
        }
        public static void GreaterThanZero<TSubject>(string name, decimal value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be positive.");
            }
        }

        public static void GreaterThanZero(string name, int value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{name} must be positive.");
            }
        }
        public static void GreaterThanZero<TSubject>(string name, int value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be positive.");
            }
        }

        public static void GreaterThanZero(string name, long value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{name} must be positive.");
            }
        }
        public static void GreaterThanZero<TSubject>(string name, long value)
        {
            if (value <= 0)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be positive.");
            }
        }

        public static void LessThanOrEqual(string name, long value, long threshold)
        {
            if (value > threshold)
            {
                throw new DomainException($"{name} must be less than {threshold}.");
            }
        }
        public static void LessThanOrEqual<TSubject>(string name, long value, long threshold)
        {
            if (value > threshold)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be less than {threshold}.");
            }
        }

        public static void LessThan(string name, long value, long threshold)
        {
            if (value >= threshold)
            {
                throw new DomainException($"{name} must be less than {threshold}.");
            }
        }
        public static void LessThan<TSubject>(string name, long value, long threshold)
        {
            if (value >= threshold)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be less than {threshold}.");
            }
        }

        public static void GreaterThan(string name, long value, long threshold)
        {
            if (value <= threshold)
            {
                throw new DomainException($"{name} must be greater than {threshold}.");
            }
        }
        public static void GreaterThan<TSubject>(string name, long value, long threshold)
        {
            if (value <= threshold)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be greater than {threshold}.");
            }
        }

        public static void GreaterThan(string name, decimal value, long threshold)
        {
            if (value <= threshold)
            {
                throw new DomainException($"{name} must be greater than {threshold}.");
            }
        }
        public static void GreaterThan<TSubject>(string name, decimal value, long threshold)
        {
            if (value <= threshold)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be greater than {threshold}.");
            }
        }

        public static void GreaterThanOrEqual(string name, long value, long threshold)
        {
            if (value < threshold)
            {
                throw new DomainException($"{name} must be greater than {threshold}.");
            }
        }
        public static void GreaterThanOrEqual<TSubject>(string name, long value, long threshold)
        {
            if (value < threshold)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} must be greater than {threshold}.");
            }
        }

        public static void NotNull(string name, object obj)
        {
            if (obj == null)
            {
                throw new DomainException($"{name} is required.");
            }
        }
        public static void NotNull<TSubject>(string name, object obj)
        {
            if (obj == null)
            {
                throw new DomainException($"{nameof(TSubject)}: {name} is required.");
            }
        }

        public static void NotNullOrEmpty(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainException($"{name} is required");
            }
        }
        public static void NotNullOrEmpty<TSubject>(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainException($"{nameof(TSubject)}: {name} is required");
            }
        }

        public static void NotNullOrWhiteSpace(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException($"{name} is required");
            }
        }
        public static void NotNullOrWhiteSpace<TSubject>(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException($"{nameof(TSubject)}: {name} is required");
            }
        }

        public static void Equal(string id1, string id2, string message)
        {
            if (id1 != id2)
            {
                throw new DomainException(message);
            }
        }
        public static void Equal<TSubject>(string id1, string id2, string message)
        {
            if (id1 != id2)
            {
                throw new DomainException($"{nameof(TSubject)}: {message}");
            }
        }

        public static void Match(string pattern, string value, string message)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(value))
            {
                throw new DomainException(message);
            }
        }
        public static void Match<TSubject>(string pattern, string value, string message)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(value))
            {
                throw new DomainException($"{nameof(TSubject)}: {message}");
            }
        }
    }
}
