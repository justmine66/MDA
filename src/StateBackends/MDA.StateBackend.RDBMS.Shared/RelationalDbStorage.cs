using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.RDBMS.Shared
{
    /// <summary>
    /// A general purpose class to work with a given relational database and ADO.NET provider.
    /// </summary>
    [DebuggerDisplay("InvariantName = {InvariantName}, ConnectionString = {ConnectionString}")]
    public class RelationalDbStorage : IRelationalDbStorage
    {
        public string InvariantName { get; }

        public string ConnectionString { get; }

        public async Task<IEnumerable<TResult>> ReadAsync<TResult>(
            string sql,
            Action<IDbCommand> parameterProvider,
            Func<IDataRecord, int, CancellationToken, Task<TResult>> selector,
            CancellationToken cancellationToken = default,
            CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return (await ExecuteAsync(sql, parameterProvider, ExecuteReaderAsync, selector, cancellationToken, commandBehavior).ConfigureAwait(false)).Item1;
        }

        public async Task<int> ExecuteAsync(
            string sql,
            Action<IDbCommand> parameterProvider,
            CancellationToken cancellationToken = default,
            CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            return (await ExecuteAsync(sql, parameterProvider, ExecuteReaderAsync, (unit, id, c) => Task.FromResult(unit), cancellationToken, commandBehavior).ConfigureAwait(false)).Item2;
        }

        /// <summary>
        /// Creates an instance of a database of type <see cref="IRelationalDbStorage"/>.
        /// </summary>
        /// <param name="invariantName">The invariant name of the connector for this database.</param>
        /// <param name="connectionString">The connection string this database should use for database operations.</param>
        /// <returns></returns>
        public static IRelationalDbStorage CreateInstance(string invariantName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(invariantName))
            {
                throw new ArgumentException("The name of invariant must contain characters", nameof(invariantName));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must contain characters", nameof(connectionString));
            }

            return new RelationalDbStorage(invariantName, connectionString);
        }

        #region [ private memembers ]

        /// <summary>
        /// Creates an instance of a database of type <see cref="RelationalDbStorage"/>.
        /// </summary>
        /// <param name="invariantName">The invariant name of the connector for this database.</param>
        /// <param name="connectionString">The connection string this database should use for database operations.</param>
        public RelationalDbStorage(string invariantName, string connectionString)
        {
            ConnectionString = connectionString;
            InvariantName = invariantName;
            SupportsCommandCancellation = DbConstantsStore.SupportsCommandCancellation(InvariantName);
            IsSynchronousAdoNetImplementation = DbConstantsStore.IsSynchronousAdoNetImplementation(InvariantName);
            _databaseCommandInterceptor = DbConstantsStore.GetDatabaseCommandInterceptor(InvariantName);
        }

        /// <summary>
        /// Command interceptor for the given data provider.
        /// </summary>
        private readonly ICommandInterceptor _databaseCommandInterceptor;

        /// <summary>
        /// If the ADO.NET provider of this storage supports cancellation or not. This
        /// capability is queried and the result is cached here.
        /// </summary>
        public bool SupportsCommandCancellation { get; }

        /// <summary>
        /// If the underlying ADO.NET implementation is natively asynchronous
        /// (the ADO.NET Db*.XXXAsync classes are overridden) or not.
        /// </summary>
        public bool IsSynchronousAdoNetImplementation { get; }

        private static async Task<Tuple<IEnumerable<TResult>, int>> SelectAsync<TResult>(
            DbDataReader reader,
            Func<IDataReader, int, CancellationToken, Task<TResult>> selector,
            CancellationToken cancellationToken)
        {
            var results = new List<TResult>();
            var resultSetIndex = 0;

            while (reader.HasRows)
            {
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    var obj = await selector(reader, resultSetIndex, cancellationToken).ConfigureAwait(false);

                    results.Add(obj);
                }

                await reader.NextResultAsync(cancellationToken).ConfigureAwait(false);

                ++resultSetIndex;
            }

            return Tuple.Create(results.AsEnumerable(), reader.RecordsAffected);
        }

        private async Task<Tuple<IEnumerable<TResult>, int>> ExecuteReaderAsync<TResult>(
            DbCommand command,
            Func<IDataRecord, int, CancellationToken, Task<TResult>> selector,
            CancellationToken cancellationToken,
            CommandBehavior commandBehavior)
        {
            using (var reader = await command.ExecuteReaderAsync(commandBehavior, cancellationToken).ConfigureAwait(false))
            {
                var cancellationRegistration = default(CancellationTokenRegistration);

                try
                {
                    if (cancellationToken.CanBeCanceled && SupportsCommandCancellation)
                    {
                        cancellationRegistration = cancellationToken.Register(CommandCancelledCallback, Tuple.Create(reader, command), false);
                    }

                    return await SelectAsync(reader, selector, cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    cancellationRegistration.Dispose();
                }
            }
        }

        private async Task<Tuple<IEnumerable<TResult>, int>> ExecuteAsync<TResult>(
            string query,
            Action<DbCommand> parameterProvider,
            Func<DbCommand,
                Func<IDataRecord, int, CancellationToken, Task<TResult>>,
                CancellationToken,
                CommandBehavior,
                Task<Tuple<IEnumerable<TResult>, int>>> executor,
            Func<IDataRecord, int, CancellationToken, Task<TResult>> selector,
            CancellationToken cancellationToken,
            CommandBehavior commandBehavior)
        {
            using (var connection = DbConnectionFactory.CreateConnection(InvariantName, ConnectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                using (var command = connection.CreateCommand())
                {
                    parameterProvider?.Invoke(command);
                    command.CommandText = query;

                    _databaseCommandInterceptor.Intercept(command);

                    var ret = IsSynchronousAdoNetImplementation
                        ? Task.Run(() => executor(command, selector, cancellationToken, commandBehavior), cancellationToken)
                        : executor(command, selector, cancellationToken, commandBehavior);

                    return await ret.ConfigureAwait(false);
                }
            }
        }

        private static void CommandCancelledCallback(object state)
        {
            //The MSDN documentation tells that DbCommand.Cancel() should not be called for SqlCommand if the reader has been closed
            //in order to avoid a race condition that would cause the SQL Server to stream the result set
            //despite the connection already closed. Source: https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlcommand.cancel(v=vs.110).aspx.
            //Enforcing this behavior across all providers does not seem to hurt.
            var stateTuple = (Tuple<DbDataReader, DbCommand>)state;

            if (!stateTuple.Item1.IsClosed)
            {
                stateTuple.Item2.Cancel();
            }
        }

        #endregion
    }
}
