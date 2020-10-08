using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.RDBMS.Shared
{
    /// <summary>
    /// 参考：https://github.com/dotnet/orleans/blob/9a58f12fee06c0b70c5ab7a0cfb22483a46f0691/src/AdoNet/Shared/Storage/IRelationalStorage.cs
    /// A common interface for all relational databases.
    /// </summary>
    public interface IRelationalDbStorage
    {
        /// <summary>
        /// Executes a given sql statement. Especially intended to use with <em>SELECT</em> statement.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="sql">The query sql statement to execute.</param>
        /// <param name="parameterProvider">Adds parameters to the query. The parameters must be in the same order with same names as defined in the query.</param>
        /// <param name="selector">This function transforms the raw <see cref="IDataRecord"/> results to type <see paramref="TResult"/> the <see cref="int"/> parameter being the result set number.</param>
        /// <param name="cancellationToken">The cancellation token. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <param name="commandBehavior">The command behavior that should be used. Defaults to <see cref="CommandBehavior.Default"/>.</param>
        /// <returns>A list of objects as a result of the <see paramref="query"/>.</returns>
        /// <example>This sample shows how to make a hand-tuned database call.
        /// // This struct holds the return value in this example.
        /// public struct Information
        /// {
        ///     public string TABLE_CATALOG { get; set; }
        ///     public string TABLE_NAME { get; set; }
        /// }
        ///
        /// Here are defined two queries. There can be more than two queries, in which case
        /// //the result sets are differentiated by a count parameter. Here the queries are
        /// //SELECT clauses, but they can be whatever, even mixed ones.
        /// <code>
        /// //This struct holds the return value in this example.
        /// public struct Information
        /// {
        ///     public string TABLE_CATALOG { get; set; }
        ///     public string TABLE_NAME { get; set; }
        /// }
        ///
        /// //Here are defined two queries. There can be more than two queries, in which case
        /// //the result sets are differentiated by a count parameter. Here the queries are
        /// //SELECT clauses, but they can be whatever, even mixed ones.
        /// IEnumerable&lt;Information&gt; ret =
        ///     await storage.ReadAsync&lt;Information&gt;("SELECT * FROM INFORMATION_SCHEMA.TABLES; SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tp1", command =>
        /// {
        ///     //Parameters are added and created like this.
        ///     //They are database vendor agnostic.
        ///     var tp1 = command.CreateParameter();
        ///     tp1.ParameterName = "tp1";
        ///     tp1.Value = "some test value";
        ///     tp1.DbType = DbType.String;
        ///     tp1.Direction = ParameterDirection.Input;
        ///     command.Parameters.Add(tp1);
        ///     
        ///     //The selector is used to select the results within the result set. In this case there are two homogenous
        ///     //result sets, so there is actually no need to check which result set the selector holds and it could
        ///     //marked with by convention by underscore (_).
        /// }, (selector, resultSetCount) =>
        ///    {
        ///        //This function is called once for each row returned, so the final result will be an
        ///        //IEnumerable&lt;Information&gt;.
        ///        return new Information
        ///        {
        ///            TABLE_CATALOG = selector.GetValueOrDefault&lt;string&gt;("TABLE_CATALOG"),
        ///            TABLE_NAME = selector.GetValueOrDefault&lt;string&gt;("TABLE_NAME")
        ///        }               
        ///}).ConfigureAwait(continueOnCapturedContext: false); 
        /// </code>
        /// </example>
        Task<IEnumerable<TResult>> ReadAsync<TResult>(
            string sql, 
            Action<IDbCommand> parameterProvider, 
            Func<IDataRecord, int, CancellationToken, Task<TResult>> selector, 
            CancellationToken cancellationToken = default, 
            CommandBehavior commandBehavior = CommandBehavior.Default);

        /// <summary>
        /// Executes a given sql statement. Especially intended to use with <em>INSERT</em>, <em>UPDATE</em>, <em>DELETE</em> or <em>DDL</em> queries.
        /// </summary>
        /// <param name="sql">The sql statement to execute.</param>
        /// <param name="parameterProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="commandBehavior"></param>
        /// <returns>Affected rows count.</returns>
        /// <example>This sample shows how to make a hand-tuned database call.
        /// <code>
        /// In contract to reading, execute queries are simpler as they return only the affected rows count if it is available.
        /// var sql = ""IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Test') CREATE TABLE Test(Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL);"
        /// int affectedRowsCount = await storage.ExecuteAsync(sql, command =>
        /// {
        ///     //There aren't parameters here, but they'd be added like when reading.
        ///     //As the affected rows count is the only thing returned, there isn't
        ///     //facilities to read anything.
        /// }).ConfigureAwait(continueOnCapturedContext: false);                
        /// </code>
        /// </example>
        Task<int> ExecuteAsync(
            string sql, 
            Action<IDbCommand> parameterProvider, 
            CancellationToken cancellationToken = default, 
            CommandBehavior commandBehavior = CommandBehavior.Default);

        /// <summary>
        /// The well known invariant name of the underlying database.
        /// </summary>
        string InvariantName { get; }

        /// <summary>
        /// The connection string used to connect to the database.
        /// </summary>
        string ConnectionString { get; }
    }
}
