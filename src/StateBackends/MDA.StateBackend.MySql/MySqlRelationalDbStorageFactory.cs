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

        public IRelationalDbStorage CreateRelationalDbStorage(DatabaseScheme scheme)
        {
            var connectionStrings = _options.ConnectionStrings;

            switch (scheme)
            {
                case DatabaseScheme.ReadDb:
                    return new RelationalDbStorage(AdoNetInvariants.InvariantNameMySqlConnector, connectionStrings.ReadDb);
                case DatabaseScheme.StateDb:
                    return new RelationalDbStorage(AdoNetInvariants.InvariantNameMySqlConnector, connectionStrings.StateDb);
                default:
                    throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null);
            }
        }
    }
}
