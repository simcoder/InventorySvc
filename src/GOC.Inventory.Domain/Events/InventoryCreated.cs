using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class InventoryCreated : IDomainEvent
    {
        public DateTime DateOccurredUtc { get; private set; }

        public AggregatesModels.InventoryAggregate.Inventory Inventory { get; private set; }

        public InventoryCreated(AggregatesModels.InventoryAggregate.Inventory inventory, DateTime dateAdded)
        {
            Inventory = inventory;
            DateOccurredUtc = dateAdded;
        }

        //required for SimpleInjector
        private InventoryCreated(){}
    }
}
