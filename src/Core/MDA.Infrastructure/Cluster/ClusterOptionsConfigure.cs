using Microsoft.Extensions.Options;

namespace MDA.Infrastructure.Cluster
{
    public class ClusterOptionsConfigure : ConfigureOptions<ClusterOptions>
    {
        public ClusterOptionsConfigure(Environment environment)
            : base(options => options.Environment = environment)
        {
        }
    }
}
