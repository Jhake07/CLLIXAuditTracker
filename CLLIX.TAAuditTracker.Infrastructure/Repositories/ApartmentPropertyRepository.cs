using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Domain;
using CLLIX.TAAuditTracker.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class ApartmentPropertyRepository(InfrastructureDbContext context) : GenericRepository<ApartmentProperty>(context), IApartmentPropertyRepository
    {
        public async Task<List<ApartmentProperty>> GetByNameApartment(string name)
        {
            var apartments = await _context.ApartmentProperties
                .Where(x => x.ApartmentName.Contains(name))
                .ToListAsync();

            if (apartments == null || apartments.Count == 0)
            {
                throw new NotFoundException(nameof(ApartmentProperty), name);
            }

            return apartments;
        }

        public async Task<ApartmentProperty?> CheckExistingApartmentName(string name)
        {
            return await _context.ApartmentProperties
                .FirstOrDefaultAsync(x => x.ApartmentName.ToLower() == name.ToLower());
        }

    }
}
