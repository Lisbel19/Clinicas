using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ClinicaDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
    }
}
