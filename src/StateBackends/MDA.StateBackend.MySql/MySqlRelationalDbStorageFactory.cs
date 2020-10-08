using MDA.StateBackend.RDBMS.Shared;
using Microsoft.Extensions.Options;
using System;

namespace MDA.StateBackend.MySql
{
    public class MySqlRelationalDbStorageFactory : IRelationalDbStorageFactory
    {
        private readonly MySqlStateBackendOptions _options;

        public MySqlRelationalDbStorageFactory(IOptions<MySqlStateBackendOptions> options)
        {
            _options = options.Value;
        }

        public IRelationalDbStorage CreateRelationalDbStorage()
        {
            var connectionString = _options.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must contain characters", nameof(connectionString));
            }

            return new RelationalDbStorage(AdoNetInvariants.InvariantNameMySqlConnector, connectionString);
        }
    }
}
