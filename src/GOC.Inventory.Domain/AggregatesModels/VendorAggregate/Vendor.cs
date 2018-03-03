using System;
using System.Collections.Generic;
using GOC.Inventory.Domain.AggregatesModels.ValueObjects;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.Domain.AggregatesModels.VendorAggregate
{
    public class Vendor : Entity<Guid> , IAggregateRoot
    {
        public string Name { get; private set; }

        public string PhoneNumber { get; private set; }

        public Address Address { get; private set; }

        public DateTime CreatedDateUtc { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public Guid? LastUpdatedUserId { get; private set; }

        public bool IsDeleted { get; private set; }

        //not persisted
        public IList<IDomainEvent> Events { get; private set; }

        public Vendor (string name, string phoneNumber, Guid userId ,Guid id) : base (id)
        {
            CreatedDateUtc = DateTime.UtcNow;
            CreatedByUserId = userId;
            Name = name;
            PhoneNumber = phoneNumber;
        }

        // required by EF
        private Vendor() : base (Guid.NewGuid())
        {
        }

        public void DeleteVendor()
        {
            IsDeleted = true;
        }

        public void EditVendor(Vendor editedVendor, Guid userId)
        {
            Name = editedVendor.Name;
            Address = editedVendor.Address;
            PhoneNumber = editedVendor.PhoneNumber;
            LastUpdatedUserId = userId;
        }
    }
}
