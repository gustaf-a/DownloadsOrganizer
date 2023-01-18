namespace DownloadsOrganizer.Data;

public class SourceFolder
{
    public SourceFolder(string path)
    {
        Path = path;
        
        ContainedFiles = new();
    }

    public string Path { get; }

    public List<SourceFile> ContainedFiles { get; set; }
}
