using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExtractApifyResults.Interfaces;

namespace ExtractApifyResults.Services
{
  public class ConsoleHostedService : IHostedService
  {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IOptions<ExtractApifyResultsConfiguration> _earConfig;
    private readonly IOptions<SecretsAppSettingsConfiguration> _appSettingsSecretsl;
    private readonly IOptions<SecretsEmailConfiguration> _emailSecrets;
    private readonly IConfiguration _config;
    private readonly IAskApifyApi _askApifyapi;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime,
        IOptions<ExtractApifyResultsConfiguration> earConfig,
        IOptions<SecretsAppSettingsConfiguration> appSettingsSecrets,
        IOptions<SecretsEmailConfiguration> emailSecrets,
        IConfiguration config,
        IAskApifyApi askApifyApi
        )
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _earConfig = earConfig;
        _appSettingsSecretsl = appSettingsSecrets;
        _emailSecrets = emailSecrets;
        _config = config;
        _askApifyapi = askApifyApi;
    }

public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                List<MemoryStream> streamsList = new List<MemoryStream>();
                try
                {
                    foreach(string task in _earConfig.Value.Tasks)
                    {
                        var lastTask = await _askApifyapi.GetLastRunningTask(task);
                        //var result = AskApifyApi.GetResults(lastTask.defaultKeyValueStoreId, "format");
                        streamsList.Add(await _askApifyapi.GetTaskResult(task, "html"));
                        streamsList.Add(await _askApifyapi.GetTaskResult(task, "xlsx"));
                    }

                    // EmailService.SendEmail(ListPriloh);



                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
                    // Stop the application once the work is done
                    _appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
  }
}