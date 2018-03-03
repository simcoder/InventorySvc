using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class VendorDeleted : IDomainEvent
    {
        public VendorDeleted(Guid id, DateTime dateDeleted, Guid userId)
        {
            VendorDeletedId = id;
            DateOccurredUtc = dateDeleted;
            UserId = userId;
        }

        public Guid VendorDeletedId { get; private set; }

        [JsonProperty]
        public DateTime DateOccurredUtc { get; private set; }

        public Guid UserId { get; private set; }

    }
}
