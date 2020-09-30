using MDA.Domain.Events;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.StateBackend.MySql
{
    public class MySqlDomainEventStateBackend : IDomainEventStateBackend
    {
        private readonly MySqlStateBackendOptions _options;

        public MySqlDomainEventStateBackend(IOptions<MySqlStateBackendOptions> options)
        {
            _options = options.Value;
        }

        public async Task AppendAsync(IDomainEvent @event, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task AppendAsync(IEnumerable<IDomainEvent> events, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(string aggregateRootId, long startOffset = 0, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IDomainEvent>> GetEventStreamAsync(string aggregateRootId, long startOffset = 0, long endOffset = long.MaxValue,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
