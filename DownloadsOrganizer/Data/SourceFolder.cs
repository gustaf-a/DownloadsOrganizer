namespace DownloadsOrganizer.Data;

public class SourceFolder
{
    public SourceFolder(string path)
    {
        FolderPath = path;

        ContainedFiles = new();
        ContainedFolders = new();
    }

    public string FolderPath { get; }

    public List<SourceFile> ContainedFiles { get; set; }
    public List<SourceFolder> ContainedFolders { get; set; }

    public bool HasIgnoredContent = false;

    public string FolderName { get { return new DirectoryInfo(FolderPath).Name; } }

    public bool IsEmpty()
        => ContainedFiles.Count == 0 && ContainedFolders.Count == 0 && !HasIgnoredContent;
}
