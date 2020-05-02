using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shopping.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.CaptureStartupErrors(true);
                webBuilder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json");
                });
                webBuilder.ConfigureLogging((context, logging) =>
                {
                    var config = context.Configuration.GetSection("Logging");
                    logging.AddConsole();
                });
            });
    }
}
