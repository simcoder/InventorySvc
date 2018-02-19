using System;
using System.Threading.Tasks;

namespace GOC.Inventory.Domain.AggregatesModels.CompanyAggregate
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyById(Guid companyId);
        Task CreateCompany(Company company);
        Task DeleteCompany(Guid companyId);
        Task EditCompany(Company editedCompany);
    }
}
