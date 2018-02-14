using System;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public CompanyRepository()
        {
        }

        public Company GetVendorById(Guid companyId)
        {
            throw new NotImplementedException();
        }
    }
}
