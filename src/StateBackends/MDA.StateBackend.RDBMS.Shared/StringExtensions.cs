namespace MDA.StateBackend.RDBMS.Shared
{
    public static class StringExtensions
    {
        public static string SetEmptyStringWhenNull(this string input) =>
            string.IsNullOrEmpty(input) ? string.Empty : input;
    }
}
