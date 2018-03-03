using System;
using System.Threading.Tasks;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class VendorRepository : RepositoryBase<VendorRepository>, IVendorRepository
    {
        readonly DatabaseContext _context;
        readonly ILogger _logger;

        public VendorRepository(DatabaseContext context,ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<VendorRepository>();
        }

        public async Task CreateVendorAsync(Vendor vendor)
        {
            try
            {
                await _context.Vendors.AddAsync(vendor);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Succesfully created vendor");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vendor");
                throw ex;
            }
        }

        public async Task DeleteVendorAsync(Guid vendorId)
        {
            try
            {
                var vendor = await ValidateVendor(vendorId);
                vendor.DeleteVendor();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vendor");
                throw ex;
            }
        }

        public async Task EditVendorAsync(Guid id, Vendor editedCompany, Guid userId)
        {
            try
            {
                var vendor = await ValidateVendor(id);
                vendor.EditVendor(vendor, userId);
                await _context.SaveChangesAsync(); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error editing vendor");
                throw ex;
            }
        }

        public async Task<Vendor> GetVendorByIdAsync(Guid vendorId)
        {
            try
            {
                return await _context.Vendors.FindAsync(vendorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company");
                throw ex;
            }
        }

        private async Task<Vendor> ValidateVendor(Guid id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                var ex = new Exception("Unable to find vendor");
                _logger.LogError(ex, "Unable to find vendor");
                throw ex;
            }
            return vendor;
        }
    }
}
