using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace ExtractApifyResults
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().Build();
            var logger = LogManager.Setup()
                                   .SetupExtensions(ext => ext.RegisterConfigSettings(config))
                                   .GetCurrentClassLogger();

            try
            {
                logger.Debug("Application Starting Up");

                await Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) => {
                        services.AddHostedService<ConsoleHostedService>();
                        services.AddOptions<ExtractApifyResultsConfiguration>().Bind(hostContext.Configuration.GetSection("ExtractApifyResults"));
                    })
                    .ConfigureLogging(logging => {
                        logging.ClearProviders();
                        logging.AddNLog(config);
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
                LogManager.Shutdown();
            }
        }
    }
}
