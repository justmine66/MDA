using System;
using System.Globalization;

namespace MDA.StateBackend.RDBMS.Shared
{
    /// <summary>
    /// Formats .NET types appropriately for database consumption in non-parameterized queries.
    /// </summary>
    public class AdoNetFormatProvider : IFormatProvider
    {
        private readonly AdoNetFormatter _formatter = new AdoNetFormatter();

        /// <summary>
        /// Returns an instance of the formatter
        /// </summary>
        /// <param name="formatType">Requested format type</param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
            => formatType == typeof(ICustomFormatter) ? _formatter : null;

        private class AdoNetFormatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                switch (arg)
                {
                    //This null check applies also to Nullable<T> when T does not have value defined.
                    case null:
                        return "NULL";
                    case string s:
                        return "N'" + s.Replace("'", "''") + "'";
                    case DateTime time:
                        return "'" + time.ToString("O") + "'";
                    case DateTimeOffset offset:
                        return "'" + offset.ToString("O") + "'";
                    case IFormattable formatter:
                        return formatter.ToString(format, CultureInfo.InvariantCulture);
                    default:
                        return arg.ToString();
                }
            }
        }
    }
}
