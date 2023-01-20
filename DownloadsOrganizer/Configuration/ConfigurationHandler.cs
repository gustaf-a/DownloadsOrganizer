using Microsoft.Extensions.Configuration;
using Serilog;

namespace DownloadsOrganizer.Configuration;

public class ConfigurationHandler : IConfigurationHandler
{
    private readonly ApplicationOptions _applicationOptions;

    private readonly CategorizationOptions _categorizationOptions;

    public ConfigurationHandler(IConfiguration configuration)
    {
        _applicationOptions = configuration.GetSection(Configuration.ApplicationOptions.Application).Get<ApplicationOptions>();

        if (_applicationOptions == null)
        {
            Log.Error("Application options are null. Please ensure that the appconfig.json file is present and contains the correct values.");
            throw new ArgumentNullException(nameof(_applicationOptions));
        }

        _categorizationOptions = configuration.GetSection(Configuration.CategorizationOptions.Categorization).Get<CategorizationOptions>();

        if (_applicationOptions == null)
        {
            Log.Error("Categorization options are null. Please ensure that the appconfig.json file is present and contains the correct values.");
            throw new ArgumentNullException(nameof(_applicationOptions));
        }
    }

    public ApplicationOptions ApplicationOptions()
        => _applicationOptions;

    public CategorizationOptions CategorizationOptions()
        => _categorizationOptions;
}
