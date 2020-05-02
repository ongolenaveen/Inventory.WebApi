using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.SqlDbProvider;

namespace Inventory.WebApp
{
    public static class StartupExtension
    {
        public static void AddBindings(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataProviders(config);
        }
    }
}
