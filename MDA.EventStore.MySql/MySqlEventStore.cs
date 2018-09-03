using MDA.Event.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MDA.EventStore.MySql
{
    public class MySqlEventStore : IEventStore
    {
        private const string Table = "EventStream";
        private const string InsertSql = "INSERT INTO {0}(TypeName,EventBody,OccurredOn) VALUES(@TypeName,@EventBody,@OccurredOn)";
        private const string SelectSql = "SELECT `EventId`,`TypeName`,`EventBody`,`OccurredOn` FROM {0}";

        private readonly ILogger<MySqlEventStore> _logger;
        private readonly MySqlEventStoreOptions _options;

        public MySqlEventStore(ILogger<MySqlEventStore> logger, IOptions<MySqlEventStoreOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public Task<IStoredEvent> AppendAsync(IDomainEvent domainEvent)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> CountStoredEventsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IStoredEvent[]> GetAllStoredEventsBetweenAsync(long lowStoredEventId, long highStoredEventId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IStoredEvent[]> GetAllStoredEventsSinceAsync(long storedEventId)
        {
            throw new System.NotImplementedException();
        }
    }
}
