using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class CompanyDeleted : IDomainEvent
    {
        
        public CompanyDeleted(Guid id, DateTime dateDeleted, Guid userId)
        {
            CompanyDeletedId = id;
            DateOccurredUtc = dateDeleted;
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public Guid CompanyDeletedId { get; private set; }

        public DateTime DateOccurredUtc { get; private set; }
    }
}
