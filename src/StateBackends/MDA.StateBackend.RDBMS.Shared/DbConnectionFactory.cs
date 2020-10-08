using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace MDA.StateBackend.RDBMS.Shared
{
    /// <summary>
    /// This class caches the references to all loaded factories
    /// </summary>
    public static class DbConnectionFactory
    {
        private static readonly ConcurrentDictionary<string, CachedFactory> FactoryCache =
            new ConcurrentDictionary<string, CachedFactory>();

        private static readonly Dictionary<string, Tuple<string, string>> ProviderFactoryTypeMap =
            new Dictionary<string, Tuple<string, string>>
            {
                { AdoNetInvariants.InvariantNameSqlServer, new Tuple<string, string>("System.Data.SqlClient", "System.Data.SqlClient.SqlClientFactory") },
                { AdoNetInvariants.InvariantNameMySql, new Tuple<string, string>("MySql.Data", "MySql.Data.MySqlClient.MySqlClientFactory") },
                { AdoNetInvariants.InvariantNameOracleDatabase, new Tuple<string, string>("Oracle.ManagedDataAccess", "Oracle.ManagedDataAccess.Client.OracleClientFactory") },
                { AdoNetInvariants.InvariantNamePostgreSql, new Tuple<string, string>("Npgsql", "Npgsql.NpgsqlFactory") },
                { AdoNetInvariants.InvariantNameSqlLite, new Tuple<string, string>("Microsoft.Data.SQLite", "Microsoft.Data.SQLite.SqliteFactory") },
                { AdoNetInvariants.InvariantNameSqlServerDotnetCore, new Tuple<string, string>("Microsoft.Data.SqlClient", "Microsoft.Data.SqlClient.SqlClientFactory") },
                { AdoNetInvariants.InvariantNameMySqlConnector, new Tuple<string, string>("MySqlConnector", "MySqlConnector.MySqlConnectorFactory") },
            };

        private static CachedFactory GetFactory(string invariantName)
        {
            if (string.IsNullOrWhiteSpace(invariantName))
            {
                throw new ArgumentNullException(nameof(invariantName));
            }

            if (!ProviderFactoryTypeMap.TryGetValue(invariantName, out var providerFactoryDefinition))
                throw new InvalidOperationException($"Database provider factory with '{invariantName}' invariant name not supported.");

            Assembly asm;
            try
            {
                var asmName = new AssemblyName(providerFactoryDefinition.Item1);
                asm = Assembly.Load(asmName);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException($"Unable to find and/or load a candidate assembly for '{invariantName}' invariant name.", exc);
            }

            if (asm == null)
                throw new InvalidOperationException($"Can't find database provider factory with '{invariantName}' invariant name. Please make sure that your ADO.Net provider package library is deployed with your application.");

            var providerFactoryType = asm.GetType(providerFactoryDefinition.Item2);
            if (providerFactoryType == null)
                throw new InvalidOperationException($"Unable to load type '{providerFactoryDefinition.Item2}' for '{invariantName}' invariant name.");

            var prop = providerFactoryType.GetFields().SingleOrDefault(p => string.Equals(p.Name, "Instance", StringComparison.OrdinalIgnoreCase) && p.IsStatic);
            if (prop == null)
                throw new InvalidOperationException($"Invalid provider type '{providerFactoryDefinition.Item2}' for '{invariantName}' invariant name.");

            var factory = (DbProviderFactory)prop.GetValue(null);

            return new CachedFactory(factory, providerFactoryType.Name, "", providerFactoryType.AssemblyQualifiedName);
        }

        public static DbConnection CreateConnection(string invariantName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(invariantName))
            {
                throw new ArgumentNullException(nameof(invariantName));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var factory = FactoryCache.GetOrAdd(invariantName, GetFactory).Factory;
            var connection = factory.CreateConnection();

            if (connection == null)
            {
                throw new InvalidOperationException($"Database provider factory: '{invariantName}' did not return a connection object.");
            }

            connection.ConnectionString = connectionString;

            return connection;
        }

        private class CachedFactory
        {
            public CachedFactory(
                DbProviderFactory factory, 
                string factoryName, 
                string factoryDescription, 
                string factoryAssemblyQualifiedNameKey)
            {
                Factory = factory;
                _factoryName = factoryName;
                _factoryDescription = factoryDescription;
                _factoryAssemblyQualifiedNameKey = factoryAssemblyQualifiedNameKey;
            }

            /// <summary>
            /// The factory to provide vendor specific functionality.
            /// </summary>
            /// <remarks>For more about <see href="http://florianreischl.blogspot.fi/2011/08/adonet-connection-pooling-internals-and.html">ConnectionPool</see>
            /// and issues with using this factory. Take these notes into account when considering robustness!</remarks>
            public readonly DbProviderFactory Factory;

            /// <summary>
            /// The name of the loaded factory, set by a database connector vendor.
            /// </summary>
            private readonly string _factoryName;

            /// <summary>
            /// The description of the loaded factory, set by a database connector vendor.
            /// </summary>
            private readonly string _factoryDescription;

            /// <summary>
            /// The description of the loaded factory, set by a database connector vendor.
            /// </summary>
            private readonly string _factoryAssemblyQualifiedNameKey;
        }
    }
}
