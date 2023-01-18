namespace DownloadsOrganizer.Data;

public class Category
{
    public string Name { get; set; } = string.Empty;

    public List<string> FileExtensions { get; set; } = new();
}
