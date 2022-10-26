using DataLayer.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Utilities.Extensions
{
    public static class ConnectionExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SampleDbContext>(options =>
            {
                var connectionString = "ConnectionStrings:DbConnection:Development";
                options.UseSqlServer(
                   configuration[connectionString]);
            });

            return services;
        }
    }
}
