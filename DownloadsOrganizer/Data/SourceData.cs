namespace DownloadsOrganizer.Data;

public class SourceData
{
    public SourceData(List<string> rootPaths)
    {
        RootPaths = rootPaths;

        SourceFiles = new();
        SourceFolders = new();
    }
    
    public List<string> RootPaths { get; }

    public List<SourceFile> SourceFiles { get; set; }
    public List<SourceFolder> SourceFolders { get; set; }
}
