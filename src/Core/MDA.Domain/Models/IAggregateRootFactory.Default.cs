using MDA.Domain.Shared.Models;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace MDA.Domain.Models
{
    public class DefaultAggregateRootFactory : IAggregateRootFactory
    {
        private readonly ILogger _logger;

        public DefaultAggregateRootFactory(ILogger<DefaultAggregateRootFactory> logger)
        {
            _logger = logger;
        }

        public IEventSourcedAggregateRoot<TAggregateRootId> CreateAggregateRoot<TAggregateRootId>(TAggregateRootId aggregateRootId, Type aggregateRootType)
        {
            try
            {
                var instance = FormatterServices.GetUninitializedObject(aggregateRootType);

                if (instance is IEventSourcedAggregateRoot<TAggregateRootId> aggregateRoot)
                {
                    aggregateRoot.Id = aggregateRootId;

                    return aggregateRoot;
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"Creating aggregate root instance has a unknown exception: {LogFormatter.PrintException(e)}.");

                return null;
            }
        }
    }
}
