using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class ItemDeleted : IDomainEvent
    {
        public ItemDeleted(Guid id, DateTime dateDeleted)
        {
            ItemDeletedId = id;
            DateOccurredUtc = dateDeleted;
        }

        public Guid ItemDeletedId { get; private set; }

        public DateTime DateOccurredUtc { get; private set; }
    }
}
