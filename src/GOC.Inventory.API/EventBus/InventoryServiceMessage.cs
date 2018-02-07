using System;

namespace GOC.Inventory.API.EventBus
{
    public class InventoryServiceMessage
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
    }
}
