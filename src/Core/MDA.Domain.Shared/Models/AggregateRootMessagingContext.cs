using System;

namespace MDA.Domain.Shared.Models
{
    public class AggregateRootMessagingContext
    {
        private static readonly Lazy<AggregateRootMessagingContext> Instance = new Lazy<AggregateRootMessagingContext>();

        public static AggregateRootMessagingContext Singleton = Instance.Value;

        public IServiceProvider ServiceProvider { get; private set; }

        public AggregateRootMessagingContext SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            return this;
        }
    }
}