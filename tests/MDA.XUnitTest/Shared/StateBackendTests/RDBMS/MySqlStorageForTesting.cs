using MDA.StateBackend.RDBMS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.XUnitTest.Shared.StateBackendTests.RDBMS
{
    internal class MySqlStorageForTesting : RelationalDbStorageForTesting
    {
        public MySqlStorageForTesting(string connectionString)
            : base(AdoNetInvariants.InvariantNameMySqlConnector, connectionString)
        { }

        public override string CancellationTestQuery
            => "DO SLEEP(10); SELECT 1;";

        public override string CreateStreamTestTable
            => "CREATE TABLE StreamingTest(Id INT NOT NULL, StreamData LONGBLOB NOT NULL);";

        public IEnumerable<string> SplitScript(string setupScript)
            => setupScript.Replace("END$$", "END;")
                .Split(new[] { "DELIMITER $$", "DELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries);

        protected override string CreateDatabaseTemplate
            => @"CREATE DATABASE `{0}`";

        protected override string DropDatabaseTemplate
            => @"DROP DATABASE `{0}`";

        public override string DefaultConnectionString
            => "Server=localhost;Database=sys;Uid=root;Pwd=root.c0m;";

        protected override string[] SetupSqlScriptFileNames
            => new[] { "MySql-Persistence-Template.sql" };

        protected override string ExistsDatabaseTemplate =>
            "SELECT COUNT(1) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{0}'";

        protected override IEnumerable<string> ConvertToExecutableBatches(string setupScript, string databaseName)
        {
            var batches = setupScript
                .Replace("END$$", "END;")
                .Split(new[] { "DELIMITER $$", "DELIMITER ;" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            batches.Add(CreateStreamTestTable);

            return batches;
        }
    }
}
