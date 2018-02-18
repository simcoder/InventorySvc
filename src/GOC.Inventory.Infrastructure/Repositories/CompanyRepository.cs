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

        public async Task CreateCompany(Company company)
        {
            await _context.AddAsync(company);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogDebug("Succesfully created company");
            }
            catch(Exception ex)
            {
                _logger.LogDebug(ex.Message);
            }
        }

        public async Task DeleteCompany(Guid companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            company.DeleteCompany();
            await _context.SaveChangesAsync();
        }

        public async Task EditCompany(Company editedCompany)
        {
            var company = await _context.Companies.FindAsync(editedCompany.Id);
            company.EditCompany(editedCompany);
            await _context.SaveChangesAsync();
        }

        public async Task<Company> GetCompanyById(Guid companyId)
        {
            return await _context.Companies.FindAsync(companyId);
        }
    }
}
