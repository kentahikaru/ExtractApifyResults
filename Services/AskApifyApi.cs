using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ExtractApifyResults.Contracts;
using ExtractApifyResults.Interfaces;

namespace ExtractApifyResults.Services
{
    public class AskApifyApi : IAskApifyApi
    {
        private readonly string baseAddress = "https://api.apify.com/";

        private readonly IOptions<ExtractApifyResultsConfiguration> _earConfig;
        private readonly IOptions<AppSettingsSecretsConfiguration> _appSettingsSecrets;
        private readonly IConfiguration _config;
        private readonly ITransport _transport;
        public AskApifyApi(IConfiguration config,  IOptions<ExtractApifyResultsConfiguration> earConfig,
            IOptions<AppSettingsSecretsConfiguration> appSettingsSecrets, ITransport transport)
        {
            _config = config;
            _earConfig = earConfig;
            _appSettingsSecrets = appSettingsSecrets;
            _transport = transport;
        }

        public async Task<LastTaskRunContract> GetLastRunningTask(string task)
        {
            string url = baseAddress + $"v2/actor-tasks/{task}/runs/last?token={_appSettingsSecrets.Value.Token}";
            string data =  await _transport.GetDataString(url);
            LastTaskRunContract lastTask = JsonSerializer.Deserialize<LastTaskRunContract>(data);
            return lastTask;
        }

        public async Task<MemoryStream> GetTaskResult(string task, string format)
        {
            string url = baseAddress + $"v2/actor-tasks/{task}/runs/last/dataset/items?token={_appSettingsSecrets.Value.Token}&status=SUCCEEDED&format={format}";
            MemoryStream memStream =  await _transport.GetDataStream(url);
            // string test = Encoding.ASCII.GetString(memStream.ToArray());
            return memStream;
        }
    }
}