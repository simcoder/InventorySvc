using System;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class ItemCreated : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }

        [JsonProperty]
        public Item Item { get; private set; }

        public Guid UserId { get; private set; }


        public ItemCreated(Item item, DateTime dateAdded, Guid userId)
        {
            Item = item;
            DateOccurredUtc = dateAdded;
            UserId = userId;
        }

        private ItemCreated()
        {
        }
    }
}
