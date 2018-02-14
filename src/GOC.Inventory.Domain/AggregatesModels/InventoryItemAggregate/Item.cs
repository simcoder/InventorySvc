using System;
using System.Collections.Generic;
using GOC.Inventory.Domain.AggregatesModels.ValueObjects;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.Domain.AggregatesModels.InventoryAggregate
{
    public class Item : Entity<Guid>, IAggregateRoot
    {
        //not persisted
        public IList<IDomainEvent> Events { get; private set; }

        /// <summary>
        /// Gets the vendor identifier this item came from.
        /// </summary>
        /// <value>The vendor identifier.</value>
        public Guid VendorId { get; private set; }

        /// <summary>
        /// Gets the company identifier this item belongs to.
        /// </summary>
        /// <value>The company identifier.</value>
        public Guid CompanyId { get; private set; }

        /// <summary>
        /// Gets the sold to company identifier.
        /// </summary>
        /// <value>The sold to company identifier.</value>
        public Guid? SoldToCompanyId { get; private set; }

        /// <summary>
        /// Gets the item description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// If this is set then this item is a mobile phone.
        /// </summary>
        /// <value>The mobile phone.</value>
        public MobilePhone MobilePhone { get; private set; }

        public DateTime CreatedDateUtc { get; private set; }

        public int CreatedByUserId { get; private set; }

        public bool IsDeleted { get; private set; }

        public Item(Guid id, string description, Guid vendorId, int userId) : base(id)
        {
            CreatedDateUtc = DateTime.UtcNow;
            CreatedByUserId = userId;
            Description = description;
            VendorId = vendorId;
            //register domain events
            DomainEvents.Register<InventoryItemCreated>(HandleInventoryItemCreated);
            //initialize Events
            Events = new List<IDomainEvent>();
            //add event
            var inventoryItemCreatedEvent = new InventoryItemCreated(this, DateTime.UtcNow);
            Events.Add(inventoryItemCreatedEvent);
        }

        // required by EF
        private Item() : base(Guid.NewGuid())
        {
        }

        #region Behaviours


        public void DeleteItem()
        {
            IsDeleted = true;
        }

        public void SaleItem(Guid companyIdSoldTo)
        {
            SoldToCompanyId = companyIdSoldTo;
        }

        public bool IsItemSold()
        {
            return SoldToCompanyId.HasValue;
        }

        public void SetItemAsMobilePhone(MobilePhone mobilePhone)
        {
            MobilePhone = mobilePhone;
        }
        #endregion

        void HandleInventoryItemCreated(InventoryItemCreated obj)
        {
        }
    }
}
