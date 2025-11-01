using CLLIX.TAAuditTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CLLIX.TAAuditTracker.Infrastructure.DBContext
{
    public class InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options)
     : IdentityDbContext<AppUser>(options)
    {
        public DbSet<TravelAgencyAgent> TravelAgencyAgents { get; set; } = default!;
        public DbSet<TravelAgency> TravelAgencies { get; set; } = default!;
        public DbSet<BookingReservation> BookingReservations { get; set; } = default!;
        public DbSet<ApartmentProperty> ApartmentProperties { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 👈 must come first for Identity

            modelBuilder.Entity<AppUser>().ToTable("AppUsers"); // 👈 override default table name
            //modelBuilder.Entity<IdentityRole>().ToTable("Role");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                         .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public class InfrastructureDbContextFactory : IDesignTimeDbContextFactory<InfrastructureDbContext>
        {
            public InfrastructureDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<InfrastructureDbContext>();

                optionsBuilder.UseSqlServer("Server=JAKE\\SQLSERVER2019;Database=AuditTracker;User ID=sa;Password=070714;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

                return new InfrastructureDbContext(optionsBuilder.Options);
            }
        }
    }

}
