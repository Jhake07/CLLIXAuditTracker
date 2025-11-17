using CLLIX.TAAuditTracker.Domain;

namespace CLLIX.TAAuditTracker.Application.ContractInterface
{
    public interface IApartmentPropertyRepository : IGenericRepository<ApartmentProperty>
    {
        Task<List<ApartmentProperty>> GetByNameApartment(string name);
        Task<ApartmentProperty?> CheckExistingApartmentName(string name);

    }
}
