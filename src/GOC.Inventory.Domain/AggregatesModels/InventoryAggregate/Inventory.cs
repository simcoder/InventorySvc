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

        public int CreatedByUserId { get; private set; }

        public bool IsDeleted { get; private set; }

        //not persisted
        public IList<IDomainEvent> Events { get; private set; }

        public Inventory(Guid companyId, Guid id, int userId) : base(id)
        {
            CompanyId = companyId;
            CreatedByUserId = userId;
            CreatedDateUtc = DateTime.UtcNow;
            //register applicable events
            DomainEvents.Register<InventoryItemAdded>(HandleInventoryItemAdded);
            DomainEvents.Register<InventoryCreated>(HandleInventoryCreated);
            DomainEvents.Register<InventoryItemRemoved>(HandleInventoryItemRemoved);


            //initialize inventory
            Items = new List<Item>();
            //initialize Events
            Events = new List<IDomainEvent>();

            //raise inventory created event
            var inventoryCreatedEvent = new InventoryCreated(this, DateTime.UtcNow);
            Events.Add(inventoryCreatedEvent);
            //DomainEvents.RaiseAsync(inventoryCreatedEvent);
        }

        // required by EF
        private Inventory() : base(Guid.NewGuid())
        {
        }

        #region Behavious

        public void AddInventoryItem(Item item)
        {
            //validate item before adding
            //TODO

            //Add Item
            Items.Add(item);
            //raise inventory item added event
            var inventoryItemAddedEvent = new InventoryItemAdded(item, DateTime.UtcNow);
            Events.Add(inventoryItemAddedEvent);
            //DomainEvents.Raise(inventoryItemAddedEvent);
        }

        public void RemoveInventoryItem(Item item)
        {
            //verify Item exists in inventory
            if (!CheckInventoryItemExists(item.Id))
            {
                throw new ArgumentException("Thhis item does is not in this inventory", nameof(item));
            }

            Items.Remove(item);
            // adding event
            var inventoryItemRemovedEvent = new InventoryItemRemoved(item, DateTime.UtcNow);
            Events.Add(inventoryItemRemovedEvent);
        }

        public bool CheckInventoryItemExists(Guid itemId)
        {
            return Items.SingleOrDefault(x => x.Id == itemId) != null;
        }

        #endregion

        #region eventHandlers

        void HandleInventoryCreated(InventoryCreated obj)
        {
        }

        void HandleInventoryItemRemoved(InventoryItemRemoved obj)
        {
        }

        void HandleInventoryItemAdded(InventoryItemAdded obj)
        {
        }

        #endregion
    }
}
