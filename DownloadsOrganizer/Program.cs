using DownloadsOrganizer.Categorization;
using DownloadsOrganizer.Categorization.CategoriesHolder;
using DownloadsOrganizer.Categorization.FileCategorization;
using DownloadsOrganizer.Categorization.FolderCategorization;
using DownloadsOrganizer.Configuration;
using DownloadsOrganizer.IO;
using DownloadsOrganizer.SourceHandling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DownloadsOrganizer;

public class Program
{
    public static async Task Main(string[] args)
    {
        //mono support for args

        //check if configuration file in args

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
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<Application>();

        services.AddScoped<ICategorizationHandler, CategorizationHandler>();
        services.AddScoped<ICategorizedDataMover, CategorizedDataMover>();
        services.AddScoped<IFileCategorizer, FileCategorizer>();
        services.AddScoped<IFileMover, FileMover>();
        services.AddScoped<IFolderCategorizer, FolderCategorizer>();
        services.AddScoped<ISourceHandler, SourceHandler>();
        services.AddScoped<IDirectoryReader, DirectoryReader>();
        
        services.AddSingleton<ICategoriesHolder, CategoriesHolder>();
        services.AddSingleton<IConfigurationHandler, ConfigurationHandler>();
    }
}
