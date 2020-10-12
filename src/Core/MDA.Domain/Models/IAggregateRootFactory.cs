﻿using System;

namespace MDA.Domain.Models
{
    public interface IAggregateRootFactory
    {
        IEventSourcedAggregateRoot CreateAggregateRoot(Type aggregateRootType);
    }
}