using System;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class VendorEdited : IDomainEvent
    {
        public VendorEdited(Vendor vendorEdited, DateTime dateEdited, Guid userId)
        {
            VendorEditedObj = vendorEdited;
            DateOccurredUtc = dateEdited;
            UserId = userId;
        }

        public Vendor VendorEditedObj { get; private set; }

        [JsonProperty]
        public DateTime DateOccurredUtc { get; private set; }

        public Guid UserId { get; private set; }

    }
}
