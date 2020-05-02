using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopping.SqlDbProvider.Database;
using Shopping.SqlDbProvider.Interfaces;
using Shopping.SqlDbProvider.Models.CsvMappers;
using Shopping.SqlDbProvider.Models.Entities;
using Shopping.SqlDbProvider.Providers;
using Shopping.Core.Interfaces;

namespace Shopping.SqlDbProvider
{
    public static class ProviderStartup
    {
        /// <summary>
        /// Register Service Providers at Inversion Control Container 
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Registered Service Collection</returns>
        public static IServiceCollection AddDataProviders(this IServiceCollection services, IConfiguration configuration)
        {
            // Entity Frame Work Bindings
            services.AddDbContext<InventoryDatabaseContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("ShoppingDatabase"));
            });

            // Data Provider Bindings
            services.AddScoped<ICsvDataprovider<FruitEntity, FruitEntityMap>, CsvDataProvider<FruitEntity, FruitEntityMap>>();
            services.AddScoped<IInventoryDataProvider, InventoryDataProvider>();

            return services;
        }
    }
}
