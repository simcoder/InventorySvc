using System;
namespace GOC.Inventory.Domain.AggregatesModels
{
    public interface IAggregateRoot
    {
        DateTime CreatedDateUtc { get; }

        Guid CreatedByUserId { get; }

        Guid? LastUpdatedUserId { get; }

        bool IsDeleted { get; }
    }
}
