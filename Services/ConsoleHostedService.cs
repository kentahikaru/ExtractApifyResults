using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExtractApifyResults
{
  public class ConsoleHostedService : IHostedService
  {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IOptions<ExtractApifyResultsConfiguration> _configuration;
     IOptions<SecretsAppSettingsConfiguration> _appSettingsSecretsl;
    IOptions<SecretsEmailConfiguration> _emailSecrets;
    IConfiguration _config;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime,
        IOptions<ExtractApifyResultsConfiguration> configuration,
        IOptions<SecretsAppSettingsConfiguration> appSettingsSecrets,
        IOptions<SecretsEmailConfiguration> emailSecrets,
        IConfiguration config
        )
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _configuration = configuration;
        _appSettingsSecretsl = appSettingsSecrets;
        _emailSecrets = emailSecrets;
        _config = config;
    }

public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("Hello World!");
                    var test = _configuration.Value.Tasks;
                    _logger.LogInformation(test.Count.ToString());
                    // Simulate real work is being done
                    await Task.Delay(1000);
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