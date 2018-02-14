using System;
namespace GOC.Inventory.Domain.AggregatesModels
{
    public interface IAggregateRoot
    {
        DateTime CreatedDateUtc { get; }

        int CreatedByUserId { get; }

        bool IsDeleted { get; }
    }
}
