using System;

namespace GOC.Inventory.API.Application.DTOs
{
    public class InventoryDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public int UserId { get; set; }
    }
}
