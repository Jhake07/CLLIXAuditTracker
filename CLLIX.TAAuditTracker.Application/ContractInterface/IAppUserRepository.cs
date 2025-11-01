using CLLIX.TAAuditTracker.Domain;
namespace CLLIX.TAAuditTracker.Application.ContractInterface
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByIdAsync(string userId);
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByUsernameAsync(string username);
        Task AddAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(AppUser user);

    }
}
