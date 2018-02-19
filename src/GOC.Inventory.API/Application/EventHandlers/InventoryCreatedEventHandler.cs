using System.Threading.Tasks;
using GOC.Inventory.API.Application.Helpers;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.Domain;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.API.Application.EventHandlers
{
    public class InventoryCreatedEventHandler : IHandle<InventoryCreated>
    {
        private readonly IEventPublisher _eventPub;

        public InventoryCreatedEventHandler(IEventPublisher eventPub)
        {
            _eventPub = eventPub;
        }

        public async Task HandleAsync(InventoryCreated args)
        {
            var serializedMessage = GocJsonHelper.SerializeJson(args);
            await _eventPub.PublishAsync(serializedMessage);
        }
    }
}
