namespace DownloadsOrganizer.Configuration;

public interface IConfigurationHandler
{
    public ApplicationOptions ApplicationOptions();
    public CategorizationOptions CategorizationOptions();
}
