using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.SqlDbProvider.Database;
using Inventory.SqlDbProvider.Interfaces;
using Inventory.SqlDbProvider.Models.CsvMappers;
using Inventory.SqlDbProvider.Models.Entities;
using Inventory.SqlDbProvider.Providers;
using Inventory.Core.Interfaces;

namespace Inventory.SqlDbProvider
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
