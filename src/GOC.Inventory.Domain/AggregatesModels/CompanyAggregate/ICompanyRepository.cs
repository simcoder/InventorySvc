using System;
using System.Threading.Tasks;

namespace GOC.Inventory.Domain.AggregatesModels.CompanyAggregate
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyByIdAsync(Guid companyId);
        Task CreateCompanyAsync(Company company);
        Task DeleteCompanyAsync(Guid companyId);
        Task EditCompanyAsync(Company editedCompany, Guid userId);
    }
}
