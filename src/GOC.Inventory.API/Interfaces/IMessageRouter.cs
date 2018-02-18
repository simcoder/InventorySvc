using System;
namespace GOC.Inventory.API.Interfaces
{
    public interface IMessageRouter
    {
        void Route(string message);
    }
}
