using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace MDA.Domain.Models
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly ILogger _logger;

        public AggregateRootFactory(ILogger<AggregateRootFactory> logger)
        {
            _logger = logger;
        }

        public IEventSourcedAggregateRoot CreateAggregateRoot(string aggregateRootId, Type aggregateRootType)
        {
            try
            {
                var instance = FormatterServices.GetUninitializedObject(aggregateRootType);

                if (instance is IEventSourcedAggregateRoot aggregateRoot)
                {
                    aggregateRoot.Id = aggregateRootId;

                    return aggregateRoot;
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create aggregate root, reason: {e}.");

                return null;
            }
        }
    }
}
