using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ExtractApifyResults.Contracts;
using ExtractApifyResults.Contracts.LastTaskRunContract;
using ExtractApifyResults.Contracts.TaskObject;
using ExtractApifyResults.Interfaces;
using Serilog;

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
            Log.Debug($"Querying: {url}");
            string data =  await _transport.GetDataString(url);
            Log.Debug($"Result: {data}");
            LastTaskRunContract lastTask = JsonSerializer.Deserialize<LastTaskRunContract>(data);
            return lastTask;
        }


        public async Task<ApifyTaskResult> GetApifyTaskResult(string task, MimeTypeEnum format)
        {
            ApifyTaskResult  atr = new ApifyTaskResult();
            atr.TaskResult = await GetTaskResult(task, format);
            atr.MimeType = format;
            var taskObject =  await GetTaskObject(task);
            atr.Name = taskObject.data.name;

            return atr;
        }

        public async Task<MemoryStream> GetTaskResult(string task, MimeTypeEnum format)
        {
            string url = baseAddress + $"v2/actor-tasks/{task}/runs/last/dataset/items?token={_appSettingsSecrets.Value.Token}&status=SUCCEEDED&format={format}";
            Log.Debug($"Querying: {url}");
            MemoryStream memStream =  await _transport.GetDataStream(url);
            // string test = Encoding.ASCII.GetString(memStream.ToArray());
            return memStream;
        }

        public async Task<TaskObject> GetTaskObject(string task)
        {
            string url = baseAddress + $"v2/actor-tasks/{task}?token={_appSettingsSecrets.Value.Token}";
            Log.Debug($"Querying: {url}");
            string data =  await _transport.GetDataString(url);
            Log.Debug($"Result: {data}");
            TaskObject taskObject = JsonSerializer.Deserialize<TaskObject>(data);
            return taskObject;
        }
    }
}