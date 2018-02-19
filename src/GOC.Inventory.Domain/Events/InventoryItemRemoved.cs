﻿using System;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class InventoryItemRemoved : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }
        public Item ItemRemoved { get; private set; }

        public InventoryItemRemoved(Item item, DateTime dateRemoved)
        {
            ItemRemoved = item;
            DateOccurredUtc = dateRemoved;
        }
        //required by Simple Injector
        private InventoryItemRemoved()
        {
        }
    }
}