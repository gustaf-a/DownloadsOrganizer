namespace DownloadsOrganizer.Data;

public class SourceData
{
    public SourceData(string rootPath)
    {
        RootPath = rootPath;

        SourceFiles = new();
        SourceFolders = new();
    }
    public string RootPath { get; }

    public List<SourceFile> SourceFiles { get; set; }
    public List<SourceFolder> SourceFolders { get; set; }
}
