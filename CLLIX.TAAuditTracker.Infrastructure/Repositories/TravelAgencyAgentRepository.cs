using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Domain;
using CLLIX.TAAuditTracker.Infrastructure.DBContext;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class TravelAgencyAgentRepository(InfrastructureDbContext context) : GenericRepository<TravelAgencyAgent>(context), ITravelAgencyAgentRepository
    {
    }
}
