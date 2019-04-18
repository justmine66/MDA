﻿using System.Collections.Generic;
using Basket.Domain.Aggregates.BasketAggregate;
using MDA.Core.EventSourcing;

namespace Basket.Domain.Events
{
    public class ModifyBasket : DomainEvent
    {
        public ModifyBasket(string buyerId, List<BasketItem> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        public string BuyerId { get; }
        public List<BasketItem> Items { get; }
    }
}
