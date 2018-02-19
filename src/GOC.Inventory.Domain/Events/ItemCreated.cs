using System;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class ItemCreated : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }

        public Item Item { get; private set; }

        public ItemCreated(Item item, DateTime dateAdded)
        {
            Item = item;
            DateOccurredUtc = dateAdded;
        }

        private ItemCreated()
        {
        }
    }
}
