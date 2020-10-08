using System;

namespace MDA.XUnitTest.Shared.Format
{
    public class PersonFormatter : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is Person person)
            {
                switch (format)
                {
                    case "CH": return $"{person.LastName} {person.FirstName}";
                    case "EN": return $"{person.FirstName} {person.LastName}";
                    default: return $"{person.LastName} {person.FirstName}";
                }
            }

            return arg.ToString();
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;

            return null;
        }
    }
}
