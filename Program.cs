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


            await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => {
                services.AddHostedService<ConsoleHostedService>();
            })
            .ConfigureLogging(logging => {
                logging.ClearProviders();
                logging.AddNLog(config);
            })
            .RunConsoleAsync();
        }
    }
}
