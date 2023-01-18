using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DownloadsOrganizer;

public class Application : IHostedService
{
    private readonly ApplicationOptions _applicationOptions;

    public Application(IConfiguration configuration)
    {
        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();

        if (_applicationOptions == null)
            throw new ArgumentNullException(nameof(_applicationOptions));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        return;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
