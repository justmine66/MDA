using System;

namespace MDA.Domain.Models
{
    public class AggregateRootMessagingContext
    {
        public static Lazy<AggregateRootMessagingContext> Instance = new Lazy<AggregateRootMessagingContext>();

        public IServiceProvider ServiceProvider { get; private set; }

        public AggregateRootMessagingContext SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            return this;
        }
    }
}