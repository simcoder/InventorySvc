using System;
using System.Threading.Tasks;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        readonly DatabaseContext _context;
        readonly ILogger _logger;

        public CompanyRepository(DatabaseContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<CompanyRepository>();
        }

        public async Task CreateCompanyAsync(Company company)
        {
            try
            {
                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Succesfully created company");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error creating company");
                throw ex;
            }
        }

        public async Task DeleteCompanyAsync(Guid companyId)
        {
            try
            {
                var company = await ValidateCompany(companyId);
                company.DeleteCompany();
                await _context.SaveChangesAsync();  
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting company");
                throw ex;
            }
        }

        public async Task EditCompanyAsync(Company editedCompany, Guid userId)
        {
            try
            {
                var company = await ValidateCompany(editedCompany.Id);
                company.EditCompany(editedCompany, userId);
                await _context.SaveChangesAsync(); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error editing company");
                throw ex;
            }
        }

        public async Task<Company> GetCompanyByIdAsync(Guid companyId)
        {
            try
            {
                return await _context.Companies.FindAsync(companyId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting company");
                throw ex;
            }
        }

        private async Task<Company> ValidateCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                var ex = new Exception("Unable to find company");
                _logger.LogError(ex, "Unable to find company");
                throw ex;
            }
            return company;
        }
    }
}
