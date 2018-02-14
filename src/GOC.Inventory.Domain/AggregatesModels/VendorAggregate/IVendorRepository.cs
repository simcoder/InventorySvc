using System;
namespace GOC.Inventory.Domain.AggregatesModels.VendorAggregate
{
    public interface IVendorRepository
    {
        Vendor GetVendorById(Guid vendorId);
    }
}
