using System;

namespace MDA.XUnitTest.Shared.Format
{
    public class Person : IFormattable
    {
        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "CH": return $"{LastName} {FirstName}";
                case "EN": return $"{FirstName} {LastName}";
                default: return $"{LastName} {FirstName}";
            }
        }
    }
}
