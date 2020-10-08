using System.Data;

namespace MDA.StateBackend.RDBMS.Shared
{
    public class NoOpCommandInterceptor : ICommandInterceptor
    {
        public static readonly ICommandInterceptor Instance = new NoOpCommandInterceptor();

        private NoOpCommandInterceptor()
        {

        }

        public void Intercept(IDbCommand command)
        {
            //NOP
        }
    }
}
