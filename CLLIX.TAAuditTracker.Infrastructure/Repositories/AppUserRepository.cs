using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Domain;
using CLLIX.TAAuditTracker.Infrastructure.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly InfrastructureDbContext _context;

        public AppUserRepository(UserManager<AppUser> userManager, InfrastructureDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<AppUser?> GetByIdAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task AddAsync(AppUser user)
        {
            await _userManager.CreateAsync(user);
        }

        public async Task UpdateAsync(AppUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAsync(AppUser user)
        {
            await _userManager.DeleteAsync(user);
        }
    }
}