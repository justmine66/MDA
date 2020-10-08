using System.Data;

namespace MDA.StateBackend.RDBMS.Shared
{
    public interface ICommandInterceptor
    {
        void Intercept(IDbCommand command);
    }
}
