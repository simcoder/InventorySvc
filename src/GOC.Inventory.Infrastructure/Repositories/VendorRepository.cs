using System;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class VendorRepository : RepositoryBase<VendorRepository>, IVendorRepository
    {
        public VendorRepository(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public Vendor GetVendorById(Guid vendorId)
        {
            throw new NotImplementedException();
        }
    }
}
