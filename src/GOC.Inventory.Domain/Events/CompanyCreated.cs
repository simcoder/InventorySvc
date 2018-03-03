﻿using System;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class CompanyCreated : IDomainEvent
    {
        
        [JsonProperty()]
        public DateTime DateOccurredUtc { get; private set; }

        [JsonProperty]
        public Company CompanyCreatedObj { get; private set; }

        public Guid UserId { get; private set; }

        public CompanyCreated(Company company, DateTime dateAdded, Guid userId)
        {
            UserId = userId;
            CompanyCreatedObj = company;
            DateOccurredUtc = dateAdded;
        }

        //required for SimpleInjector
        private CompanyCreated() { }
    }
}
