using System;
using System.Collections.Generic;
using GOC.Inventory.Domain.AggregatesModels.ValueObjects;
using GOC.Inventory.Domain.Events;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.AggregatesModels.CompanyAggregate
{
    public class Company : Entity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public DateTime CreatedDateUtc { get; private set; }
        public bool IsDeleted { get; private set; }

        //needed for private setter properties
        [JsonProperty]
        public Guid CreatedByUserId { get; private set; }
        //needed for private setter properties
        [JsonProperty]
        public Guid? LastUpdatedUserId { get; private set; }

        //not persisted
        public IList<IDomainEvent> Events { get; private set; }

        public Company (Guid id, string name, string phoneNumber, Address address, Guid userId) : base (id)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            CreatedByUserId = userId;
            CreatedDateUtc = DateTime.UtcNow;
        }

        // required by EF
        private Company() : base(Guid.NewGuid())
        {
        }

        public void DeleteCompany()
        {
            IsDeleted = true;
        }

        public void EditCompany(Company editedCompany, Guid userId)
        {
            Name = editedCompany.Name;
            //Address = editedCompany.Address;
            PhoneNumber = editedCompany.PhoneNumber;
            LastUpdatedUserId = userId;
        }
    }
}
