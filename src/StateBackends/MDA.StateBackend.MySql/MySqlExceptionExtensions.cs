using System;
using MySqlConnector;

namespace MDA.StateBackend.MySql
{
    public static class MySqlExceptionExtensions
    {
        private const string IdempotentIdentifier = "Duplicate entry";

        public static bool HasDuplicateEntry(this MySqlException exception)
            => exception.Number == 1062 && exception.Message.Contains(IdempotentIdentifier);
    }
}
