using System.Threading.Tasks;
using GOC.Inventory.API.Application.Helpers;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.Domain.Events;

namespace GOC.Inventory.Domain.Handlers
{
    public class ItemAddedToInventoryHandler : IHandle<InventoryItemAdded>
    {
        private readonly IEventPublisher _eventPub;

        public ItemAddedToInventoryHandler(IEventPublisher eventPub)
        {
            _eventPub = eventPub;
        }


        public async Task HandleAsync(InventoryItemAdded args)
        {
            var serializedMessage = GocJsonHelper.SerializeJson(args);
            await _eventPub.PublishAsync(serializedMessage);
        }
    }
}
