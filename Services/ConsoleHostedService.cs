using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExtractApifyResults.Interfaces;
using ExtractApifyResults.Contracts;

namespace ExtractApifyResults.Services
{
  public class ConsoleHostedService : IHostedService
  {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IOptions<ExtractApifyResultsConfiguration> _earConfig;
    private readonly IOptions<AppSettingsSecretsConfiguration> _appSettingsSecretsl;
    private readonly IOptions<EmailSecretsConfiguration> _emailSecrets;
    private readonly IConfiguration _config;
    private readonly IAskApifyApi _askApifyapi;
    private readonly IEmail _email;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime,
        IOptions<ExtractApifyResultsConfiguration> earConfig,
        IOptions<AppSettingsSecretsConfiguration> appSettingsSecrets,
        IOptions<EmailSecretsConfiguration> emailSecrets,
        IConfiguration config,
        IAskApifyApi askApifyApi,
        IEmail email
        )
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _earConfig = earConfig;
        _appSettingsSecretsl = appSettingsSecrets;
        _emailSecrets = emailSecrets;
        _config = config;
        _askApifyapi = askApifyApi;
        _email = email;
    }

public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                List<ApifyTaskResult> tasksList = new List<ApifyTaskResult>();
                try
                {
                    foreach(string task in _earConfig.Value.Tasks)
                    {
                        var lastTask = await _askApifyapi.GetLastRunningTask(task);
                        tasksList.Add(await _askApifyapi.GetApifyTaskResult(task, MimeTypeEnum.html));
                        tasksList.Add(await _askApifyapi.GetApifyTaskResult(task, MimeTypeEnum.xlsx));
                    }

                    await _email.Send(tasksList);


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
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