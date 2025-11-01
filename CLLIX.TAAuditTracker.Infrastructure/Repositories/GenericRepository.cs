using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Application.Shared.Exceptions;
using CLLIX.TAAuditTracker.Domain;
using CLLIX.TAAuditTracker.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace CLLIX.TAAuditTracker.Infrastructure.Repositories
{
    public class GenericRepository<T>(InfrastructureDbContext context) : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly InfrastructureDbContext _context = context;

        public async Task CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);

            // If entity is null, throw a NotFoundException
            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The entity to update cannot be null.");
            }

            // Retrieve the existing entity from the database
            var existingEntity = await _context.Set<T>().FindAsync(entity.Id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with Id {entity.Id} was not found.");
            }

            // Update the fields of the existing entity
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
}
