using System;
using System.Threading.Tasks;

namespace GOC.Inventory.Domain.AggregatesModels.VendorAggregate
{
    public interface IVendorRepository
    {
        Task<Vendor> GetVendorByIdAsync(Guid vendorId);
        Task CreateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(Guid vendorId);
        Task EditVendorAsync(Guid id, Vendor editedCompany, Guid userId);
    }
}
