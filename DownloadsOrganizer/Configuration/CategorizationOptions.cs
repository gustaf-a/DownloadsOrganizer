using DownloadsOrganizer.Data;

namespace DownloadsOrganizer.Configuration;

public class CategorizationOptions
{
    public const string Categorization = "Categorization";

    public string[] IgnoreFolderNames { get; set; } = Array.Empty<string>();
    public string[] IgnorePrefixes { get; set; } = Array.Empty<string>();
    public string[] IgnoreFileNames { get; set; } = Array.Empty<string>();
    public string[] IgnoreFileExtensions { get; set; } = Array.Empty<string>();

    public Category[] Categories { get; set; } = Array.Empty<Category>();
}
