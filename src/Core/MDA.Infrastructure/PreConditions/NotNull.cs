using System;

namespace MDA.Infrastructure.PreConditions
{
    public class NotNull<T>
    {
        public NotNull(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public static implicit operator NotNull<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException();

            return new NotNull<T>(value);
        }

        public static implicit operator T(NotNull<T> value) => value.Value;
    }
}
