using System;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class InventoryItemAdded : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }

        public Item ItemAdded { get; private set; }

        public Guid UserId { get; private set; }


        public InventoryItemAdded (Item item, DateTime dateAdded, Guid userId)
        {
            ItemAdded = item;
            DateOccurredUtc = dateAdded;
            UserId = userId;
        }

        //required by simple injector
        private InventoryItemAdded()
        {
        }
    }
}
