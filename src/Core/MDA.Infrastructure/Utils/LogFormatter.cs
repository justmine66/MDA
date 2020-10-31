using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace MDA.Infrastructure.Utils
{
    public static class LogFormatter
    {
        private const string TimeFormat = "HH:mm:ss.fff 'GMT'"; // Example: 09:50:43.341 GMT
        private const string DateFormat = "yyyy-MM-dd " + TimeFormat; // Example: 2010-09-02 09:50:43.341 GMT - Variant of UniversalSorta­bleDateTimePat­tern

        /// <summary>
        /// Utility function to convert a <c>DateTime</c> object into printable data format used by the Logger subsystem.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> value to be printed.</param>
        /// <returns>Formatted string representation of the input data, in the printable format used by the Logger subsystem.</returns>
        public static string PrintDate(DateTime date)
        {
            return date.ToString(DateFormat, CultureInfo.InvariantCulture);
        }

        public static DateTime ParseDate(string dateStr)
        {
            return DateTime.ParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert a <c>DateTime</c> object into printable time format used by the Logger subsystem.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> value to be printed.</param>
        /// <returns>Formatted string representation of the input data, in the printable format used by the Logger subsystem.</returns>
        public static string PrintTime(DateTime date)
        {
            return date.ToString(TimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert an exception into printable format, including expanding and formatting any nested sub-expressions.
        /// </summary>
        /// <param name="exception">The exception to be printed.</param>
        /// <returns>Formatted string representation of the exception, including expanding and formatting any nested sub-expressions.</returns>
        public static string PrintException(Exception exception)
        {
            return exception == null ? string.Empty : PrintException_Helper(exception, 0, true);
        }

        public static string PrintExceptionWithoutStackTrace(Exception exception)
        {
            return exception == null ? string.Empty : PrintException_Helper(exception, 0, false);
        }

        private static string PrintException_Helper(Exception exception, int level, bool includeStackTrace)
        {
            if (exception == null) return string.Empty;

            var sb = new StringBuilder();

            sb.Append(PrintOneException(exception, level, includeStackTrace));

            switch (exception)
            {
                case ReflectionTypeLoadException loadException:
                {
                    var loaderExceptions = loadException.LoaderExceptions;
                    if (loaderExceptions == null || loaderExceptions.Length == 0)
                    {
                        sb.Append("No LoaderExceptions found");
                    }
                    else
                    {
                        foreach (var inner in loaderExceptions)
                        {
                            // call recursively on all loader exceptions. Same level for all.
                            sb.Append(PrintException_Helper(inner, level + 1, includeStackTrace));
                        }
                    }

                    break;
                }

                case AggregateException aggregateException:
                {
                    var innerExceptions = aggregateException.InnerExceptions;

                    foreach (var inner in innerExceptions)
                    {
                        // call recursively on all inner exceptions. Same level for all.
                        sb.Append(PrintException_Helper(inner, level + 1, includeStackTrace));
                    }

                    break;
                }

                default:
                {
                    if (exception.InnerException != null)
                    {
                        // call recursively on a single inner exception.
                        sb.Append(PrintException_Helper(exception.InnerException, level + 1, includeStackTrace));
                    }

                    break;
                }
            }
            return sb.ToString();
        }

        private static string PrintOneException(Exception exception, int level, bool includeStackTrace)
        {
            if (exception == null) return string.Empty;

            var stack = string.Empty;

            if (includeStackTrace && exception.StackTrace != null)
                stack = string.Format(Environment.NewLine + exception.StackTrace);

            var message = exception.Message;

            return string.Format(Environment.NewLine + "Exc level {0}: {1}: {2}{3}",
                level,
                exception.GetType(),
                message,
                stack);
        }
    }
}
