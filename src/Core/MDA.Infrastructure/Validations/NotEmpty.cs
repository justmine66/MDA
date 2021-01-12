using System;

namespace MDA.Infrastructure.Validations
{
    public class NotEmpty : Attribute
    {
        public NotEmpty(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static implicit operator NotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException();

            return new NotEmpty(value);
        }

        public static implicit operator string(NotEmpty value) => value.Value;
    }
}
