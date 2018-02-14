using System;
namespace GOC.Inventory.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime DateOccurredUtc { get; }
    }
}
