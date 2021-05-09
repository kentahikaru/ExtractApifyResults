using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExtractApifyResults
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => {

            })
            .ConfigureServices((hostContext, services) => {
                services.AddHostedService<ConsoleHostedService>();
            })
            .RunConsoleAsync();
        }
    }
}
