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

        /// <summary>
        /// Gets the created date UTC.
        /// </summary>
        /// <value>The created date UTC.</value>
        public DateTime CreatedDateUtc { get; private set; }

        /// <summary>
        /// Gets the created by user identifier.
        /// </summary>
        /// <value>The created by user identifier.</value>
        public int CreatedByUserId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:GOC.Inventory.Domain.AggregatesModels.InventoryAggregate.Item"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; private set; }


        public Item(Guid id, string description, Guid vendorId, int userId) : base(id)
        {
            CreatedByUserId = userId;
            Description = description;
            VendorId = vendorId;
            //register domain events
            DomainEvents.Register<ItemCreated>(HandleItemCreated);
            DomainEvents.Register<ItemDeleted>(HandleItemDeleted);
            //initialize Events
            Events = new List<IDomainEvent>();
            //add event
            var inventoryItemCreatedEvent = new ItemCreated(this, DateTime.UtcNow);
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
            var inventoryItemDeleted = new ItemDeleted(this.Id, DateTime.UtcNow);
            Events.Add(inventoryItemDeleted);
        }

        public void SaleItem(Guid companyIdSoldTo)
        {
            SoldToCompanyId = companyIdSoldTo;
            //TODO add event
        }

        public bool IsItemSold()
        {
            return SoldToCompanyId.HasValue;
        }

        public void SetItemAsMobilePhone(MobilePhone mobilePhone)
        {
            MobilePhone = mobilePhone;
            //TODO add event
        }

        #endregion

        void HandleItemCreated(ItemCreated obj)
        {
            CreatedDateUtc = obj.DateOccurredUtc;
        }
        void HandleItemDeleted(ItemDeleted obj)
        {
            DeleteItem();
        }
    }
}
