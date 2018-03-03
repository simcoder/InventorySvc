using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class InventoryCreated : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }

        [JsonProperty]
        public AggregatesModels.InventoryAggregate.Inventory Inventory { get; private set; }

        public Guid UserId { get; private set; }

        public InventoryCreated(AggregatesModels.InventoryAggregate.Inventory inventory, DateTime dateAdded, Guid userdId)
        {
            Inventory = inventory;
            DateOccurredUtc = dateAdded;
            UserId = userdId;
        }

        //required for SimpleInjector
        private InventoryCreated(){}
    }
}
