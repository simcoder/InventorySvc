using System;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class CompanyEdited : IDomainEvent
    {
        
        public CompanyEdited(Company companyEdited, DateTime dateEdited)
        {
            CompanyEditedObj = companyEdited;
            DateOccurredUtc = dateEdited;
        }

        public Company CompanyEditedObj { get; private set; }

        public DateTime DateOccurredUtc { get; private set; }

    }
}
