using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Domain;
using CLLIX.TAAuditTracker.Infrastructure.DBContext;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class ApartmentPropertyRepository(InfrastructureDbContext context) : GenericRepository<ApartmentProperty>(context), IApartmentPropertyRepository
    {
    }
}
