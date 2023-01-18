namespace DownloadsOrganizer.Data;

public class SourceFolder
{
    public SourceFolder(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public List<SourceFile> ContainedFiles { get; set; }
}
