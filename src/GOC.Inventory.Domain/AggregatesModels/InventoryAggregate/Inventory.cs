using System;
using System.Collections.Generic;
using System.Linq;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.Domain.AggregatesModels.InventoryAggregate
{
    public class Inventory : Entity<Guid>, IAggregateRoot
    {
        public IList<Item> Items { get; private set; }

        public Guid CompanyId { get; private set; }

        public DateTime CreatedDateUtc { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public Guid? LastUpdatedUserId { get; private set; }


        public bool IsDeleted { get; private set; }

        //not persisted
        public IList<IDomainEvent> Events { get; private set; }

        public Inventory(Guid companyId, Guid id, Guid userId) : base(id)
        {
            CompanyId = companyId;
            CreatedByUserId = userId;
            CreatedDateUtc = DateTime.UtcNow;

            //initialize inventory
            Items = new List<Item>();
            //initialize Events
            Events = new List<IDomainEvent>();

            //raise inventory created event
            var inventoryCreatedEvent = new InventoryCreated(this, DateTime.UtcNow, userId);
            Events.Add(inventoryCreatedEvent);
        }

        // required by EF
        private Inventory() : base(Guid.NewGuid())
        {
        }

        #region Behavious

        public void AddInventoryItem(Item item, Guid userId)
        {
            //validate item before adding
            //TODO
            LastUpdatedUserId = userId;
            //Add Item
            Items.Add(item);
            //raise inventory item added event
            var inventoryItemAddedEvent = new InventoryItemAdded(item, DateTime.UtcNow, userId);
            Events.Add(inventoryItemAddedEvent);
        }

        public void RemoveInventoryItem(Item item, Guid userId)
        {
            //verify Item exists in inventory
            if (!CheckInventoryItemExists(item.Id))
            {
                throw new ArgumentException("Thhis item does is not in this inventory", nameof(item));
            }
            LastUpdatedUserId = userId;
            Items.Remove(item);
            // adding event
            var inventoryItemRemovedEvent = new InventoryItemRemoved(item, DateTime.UtcNow, userId);
            Events.Add(inventoryItemRemovedEvent);
        }

        public bool CheckInventoryItemExists(Guid itemId)
        {
            return Items.SingleOrDefault(x => x.Id == itemId) != null;
        }

        #endregion

    }
}
