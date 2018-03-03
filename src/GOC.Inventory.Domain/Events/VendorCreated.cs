using System;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class VendorCreated : IDomainEvent
    {
        [JsonProperty]
        public DateTime DateOccurredUtc { get; private set; }

        [JsonProperty]
        public Vendor VendorCreatedObj { get; private set; }

        public Guid UserId { get; private set; }


        public VendorCreated(Vendor vendor, DateTime dateAdded, Guid userId)
        {
            VendorCreatedObj = vendor;
            DateOccurredUtc = dateAdded;
            UserId = userId;
        }

        //required for SimpleInjector
        private VendorCreated() { }
    }
}
