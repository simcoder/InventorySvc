using System.Threading.Tasks;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.Domain
{
    public interface IHandle<T> where T : IDomainEvent
    {
        Task HandleAsync(T args);
    }
}
