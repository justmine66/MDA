﻿using MDA.StateBackend.RDBMS.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.XUnitTest.Shared.StateBackendTests.RDBMS
{
    public abstract class RelationalDbStorageForTesting
    {
        private static readonly Dictionary<string, Func<string, RelationalDbStorageForTesting>> InstanceFactory =
            new Dictionary<string, Func<string, RelationalDbStorageForTesting>>
            {
                {AdoNetInvariants.InvariantNameMySqlConnector, cs => new MySqlStorageForTesting(cs)},
            };

        public IRelationalDbStorage Storage { get; private set; }

        public string CurrentConnectionString => Storage.ConnectionString;

        /// <summary>
        /// Default connection string for testing
        /// </summary>
        public abstract string DefaultConnectionString { get; }

        /// <summary>
        /// A delayed query that is then cancelled in a test to see if cancellation works.
        /// </summary>
        public abstract string CancellationTestQuery { get; }

        public abstract string CreateStreamTestTable { get; }

        public virtual string DeleteStreamTestTable => "DELETE StreamingTest;";

        public virtual string StreamTestSelect => "SELECT Id, StreamData FROM StreamingTest WHERE Id = @streamId;";

        public virtual string StreamTestInsert => "INSERT INTO StreamingTest(Id, StreamData) VALUES(@id, @streamData);";

        /// <summary>
        /// The script that creates Orleans schema in the database, usually CreateOrleansTables_xxxx.sql
        /// </summary>
        protected abstract string[] SetupSqlScriptFileNames { get; }

        /// <summary>
        /// A query template to create a database with a given name.
        /// </summary>
        protected abstract string CreateDatabaseTemplate { get; }

        /// <summary>
        /// A query template to drop a database with a given name.
        /// </summary>
        protected abstract string DropDatabaseTemplate { get; }

        /// <summary>
        /// A query template if a database with a given name exists.
        /// </summary>
        protected abstract string ExistsDatabaseTemplate { get; }

        /// <summary>
        /// Converts the given script into batches to execute sequentially.
        /// </summary>
        /// <param name="setupScript">the script. usually CreateOrleansTables_xxxx.sql</param>
        /// <param name="databaseName">the name of the database</param>
        protected abstract IEnumerable<string> ConvertToExecutableBatches(string setupScript, string databaseName);

        public static async Task<RelationalDbStorageForTesting> SetupInstance(
            string invariantName,
            string testDatabaseName,
            string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(invariantName))
            {
                throw new ArgumentException("The name of invariant must contain characters", nameof(invariantName));
            }

            if (string.IsNullOrWhiteSpace(testDatabaseName))
            {
                throw new ArgumentException("Database string must contain characters", nameof(testDatabaseName));
            }

            Console.WriteLine("Initializing relational databases...");

            var testStorage = connectionString == null
                ? CreateTestInstance(invariantName)
                : CreateTestInstance(invariantName, connectionString);

            Console.WriteLine("Dropping and recreating database '{0}' with connection string '{1}'", testDatabaseName, connectionString ?? testStorage.DefaultConnectionString);

            if (await testStorage.ExistsDatabaseAsync(testDatabaseName))
            {
                await testStorage.DropDatabaseAsync(testDatabaseName);
            }

            await testStorage.CreateDatabaseAsync(testDatabaseName);

            // The old storage instance has the previous connection string, time have a new handle with a new connection string...
            testStorage = testStorage.CopyInstance(testDatabaseName);

            Console.WriteLine("Creating database tables...");

            var setupScript = string.Empty;

            // Concatenate scripts
            foreach (var fileName in testStorage.SetupSqlScriptFileNames)
            {
                setupScript += File.ReadAllText(fileName);

                // Just in case add a CRLF between files, but they should end in a new line.
                setupScript += "\r\n";
            }

            await testStorage.ExecuteSetupScript(setupScript, testDatabaseName);

            Console.WriteLine("Initializing relational databases done.");

            return testStorage;
        }

        private static RelationalDbStorageForTesting CreateTestInstance(string invariantName, string connectionString)
            => InstanceFactory[invariantName](connectionString);

        private static RelationalDbStorageForTesting CreateTestInstance(string invariantName)
            => CreateTestInstance(invariantName, CreateTestInstance(invariantName, "not empty").DefaultConnectionString);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invariantName"></param>
        /// <param name="connectionString"></param>
        protected RelationalDbStorageForTesting(string invariantName, string connectionString)
            => Storage = RelationalDbStorage.CreateInstance(invariantName, connectionString);

        /// <summary>
        /// Executes the given script in a test context.
        /// </summary>
        /// <param name="setupScript">the script. usually CreateOrleansTables_xxxx.sql</param>
        /// <param name="dataBaseName">the target database to be populated</param>
        /// <returns></returns>
        private async Task ExecuteSetupScript(string setupScript, string dataBaseName)
        {
            var splitScripts = ConvertToExecutableBatches(setupScript, dataBaseName);

            foreach (var script in splitScripts)
            {
                var res1 = await Storage.ExecuteAsync(script);
            }
        }

        /// <summary>
        /// Checks the existence of a database using the given <see paramref="storage"/> storage object.
        /// </summary>
        /// <param name="databaseName">The name of the database existence of which to check.</param>
        /// <returns><em>TRUE</em> if the given database exists. <em>FALSE</em> otherwise.</returns>
        private async Task<bool> ExistsDatabaseAsync(string databaseName)
        {
            var ret = await Storage.ReadAsync(
                string.Format(ExistsDatabaseTemplate, databaseName),
                command => { },
                (selector, resultSetCount, cancellationToken)
                    => Task.FromResult(selector.GetBoolean(0))).ConfigureAwait(false);

            return ret.First();
        }

        /// <summary>
        /// Creates a database with a given name.
        /// </summary>
        /// <param name="databaseName">The name of the database to create.</param>
        /// <returns>The call will be successful if the DDL query is successful. Otherwise an exception will be thrown.</returns>
        private async Task CreateDatabaseAsync(string databaseName)
            => await Storage.ExecuteAsync(
                string.Format(CreateDatabaseTemplate, databaseName),
                command => { }).ConfigureAwait(false);

        /// <summary>
        /// Drops a database with a given name.
        /// </summary>
        /// <param name="databaseName">The name of the database to drop.</param>
        /// <returns>The call will be successful if the DDL query is successful. Otherwise an exception will be thrown.</returns>
        private Task DropDatabaseAsync(string databaseName)
            => Storage.ExecuteAsync(
                string.Format(DropDatabaseTemplate, databaseName),
                command => { });

        /// <summary>
        /// Creates a new instance of the storage based on the old connection string by changing the database name.
        /// </summary>
        /// <param name="newDatabaseName">Connection string instance name of the database.</param>
        /// <returns>A new <see cref="RelationalDbStorageForTesting"/> instance with having the same connection string but with a new databaseName.</returns>
        private RelationalDbStorageForTesting CopyInstance(string newDatabaseName)
        {
            var csb = new DbConnectionStringBuilder
            {
                ConnectionString = Storage.ConnectionString,
                ["Database"] = newDatabaseName
            };

            return CreateTestInstance(Storage.InvariantName, csb.ConnectionString);
        }
    }
}
