using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class CompanyDeleted : IDomainEvent
    {
        
        public CompanyDeleted(Guid id, DateTime dateDeleted)
        {
            CompanyDeletedId = id;
            DateOccurredUtc = dateDeleted;
        }

        public Guid CompanyDeletedId { get; private set; }

        public DateTime DateOccurredUtc { get; private set; }
    }
}
