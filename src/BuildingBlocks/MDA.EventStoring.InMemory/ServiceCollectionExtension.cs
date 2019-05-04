using Microsoft.Extensions.DependencyInjection;

namespace MDA.EventStoring.InMemory
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryEventStoringServices(this IServiceCollection container)
        {
            container.AddSingleton<IEventStore, InMemoryEventStore>();
            return container;
        }
    }
}
