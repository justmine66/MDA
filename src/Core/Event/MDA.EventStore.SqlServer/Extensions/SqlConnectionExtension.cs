using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MDA.EventStore.SqlServer.Extensions
{
    public static class SqlConnectionExtension
    {
        public static Task<SqlTransaction> BeginTransactionAsync(this SqlConnection connection)
        {
            return Task.Run(() => { return connection.BeginTransaction(); });
        }
    }
}
