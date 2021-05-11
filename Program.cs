using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ExtractApifyResults
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            var logger = Log.Logger;

            try
            {
                logger.Debug("Application Starting Up");

                await Host.CreateDefaultBuilder(args)
                    .UseSerilog()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureServices((hostContext, services) => {
                        services.AddHostedService<ConsoleHostedService>();
                        services.AddOptions<ExtractApifyResultsConfiguration>().Bind(hostContext.Configuration.GetSection("ExtractApifyResults"));
                        services.AddOptions<SecretsEmailConfiguration>().Bind(hostContext.Configuration.GetSection("Email"));
                        services.AddOptions<SecretsAppSettingsConfiguration>().Bind(hostContext.Configuration.GetSection("AppSettings"));
                    })
                    .ConfigureLogging(logging => {
                        logging.ClearProviders();

                    })
                    .RunConsoleAsync();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)

            }
        }
    }
}
