using System;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class InventoryItemCreated : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }

        public Item CreatedInventoryItem { get; private set; }

        public InventoryItemCreated(Item item, DateTime dateAdded)
        {
            CreatedInventoryItem = item;
            DateOccurredUtc = dateAdded;
        }

        private InventoryItemCreated()
        {
        }
    }
}
