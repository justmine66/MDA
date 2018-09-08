using System.Data.Common;
using System.Threading.Tasks;

namespace MDA.Common.Extensions
{
    public static class DbTransactionExtension
    {
        public static Task CommitAsync(this DbTransaction transaction)
        {
            return Task.Run(() => { transaction.Commit(); });
        }

        public static Task RollbackAsync(this DbTransaction transaction)
        {
            return Task.Run(() => { transaction.Rollback(); });
        }
    }
}
