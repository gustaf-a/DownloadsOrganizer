using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DownloadsOrganizer;

public class Program
{
    public static async void Main(string[] args)
    {

        Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(
            services =>
            {
                services.AddHostedService<Application>();
                ConfigureServices(services);
            }
            );

        var host = builder.Build();

        try
        {
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error when running application.");
            throw;
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<Application>();


    }
}
