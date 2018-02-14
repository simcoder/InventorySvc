using System;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        public VendorRepository()
        {
        }

        public Vendor GetVendorById(Guid vendorId)
        {
            throw new NotImplementedException();
        }
    }
}
