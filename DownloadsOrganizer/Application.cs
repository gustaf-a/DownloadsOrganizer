using Microsoft.Extensions.Hosting;

namespace DownloadsOrganizer;

public class Application : IHostedService
{
    public Application()
    {

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
