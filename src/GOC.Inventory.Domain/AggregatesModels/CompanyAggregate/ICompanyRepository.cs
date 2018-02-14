using System;
namespace GOC.Inventory.Domain.AggregatesModels.CompanyAggregate
{
    public interface ICompanyRepository
    {
        Company GetVendorById(Guid companyId);
    }
}
