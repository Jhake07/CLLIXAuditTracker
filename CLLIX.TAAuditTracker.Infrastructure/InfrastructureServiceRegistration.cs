using CLLIX.TAAuditTracker.Application.ContractInterface;
using CLLIX.TAAuditTracker.Domain;
using CLLIX.TAAuditTracker.Infrastructure.DBContext;
using CLLIX.TAAuditTracker.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CLLIX.TAAuditTracker.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddDbContext<InfrastructureDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CLIXX_AuditTracker"));
            });

            // Register Identity for AppUser
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<InfrastructureDbContext>()
                .AddDefaultTokenProviders();


            // Repositories
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApartmentPropertyRepository, ApartmentPropertyRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
            services.AddScoped<IExcelBookingParser, ExcelBookingParserRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITravelAgencyAgentRepository, TravelAgencyAgentRepository>();
            services.AddScoped<ITravelAgencyRepository, TravelAgencyRepository>();

            return services;
        }
    }
}
