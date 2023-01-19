namespace DownloadsOrganizer.Configuration;

public class ApplicationOptions
{
    public const string Application = "Application";

    public string[] SourceFolder { get; set; } = Array.Empty<string>();
    public string OutputFolder { get; set; } = string.Empty;

    public string CategoryFolderPrefix { get; set; } = "_";
}
