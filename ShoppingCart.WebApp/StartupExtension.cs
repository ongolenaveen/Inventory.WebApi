using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopping.SqlDbProvider;

namespace Shopping.WebApp
{
    public static class StartupExtension
    {
        public static void AddBindings(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataProviders(config);
        }
    }
}
